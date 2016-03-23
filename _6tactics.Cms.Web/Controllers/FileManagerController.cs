using _6tactics.Cms.Core.Attributes;
using _6tactics.Cms.Core.ViewModels.FileManager;
using _6tactics.Utilities.FileSystem;
using _6tactics.Utilities.FileSystem.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace _6tactics.Cms.Web.Controllers
{
    [Authorizer(Roles = "Administrators,AdminReadOnly")]

    public class FileManagerController : Controller
    {
        #region Index

        public ActionResult Index()
        {
            return View(new FileSystemReader(Server.MapPath("~/Content/Uploads")).GetDirectoriesTree());
        }

        [AjaxOnly]
        public ActionResult _FoldersTreeView()
        {
            return PartialView(new FileSystemReader(Server.MapPath("~/Content/Uploads")).GetDirectoriesTree());
        }

        #endregion



        #region PopupReadMode

        public ActionResult PopupReadMode(string inputIdToShowFileUrl)
        {
            return PartialView(new PopupReadModeViewModel
            {
                DirectoryTree = new FileSystemReader(Server.MapPath("~/Content/Uploads")).GetDirectoriesTree(),
                InputIdToShowFileUrl = inputIdToShowFileUrl
            });
        }

        #endregion


        #region Create

        [AjaxOnly]
        public ActionResult CreateFolder(string whereToCreate)
        {
            if (string.IsNullOrWhiteSpace(whereToCreate)) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            return PartialView(new FileManagerCreateViewModel { WhereToCreate = whereToCreate });
        }

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult DoCreatingFolder(FileManagerCreateViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.WhereToCreate) && string.IsNullOrWhiteSpace(model.FolderNameToCreate)) return Content("");

            string filePath =
                string.Concat(FileSystemUtilities.ChangePathMinusesToForwardSlashes(model.WhereToCreate), "/",
                    model.FolderNameToCreate);

            FileSystemWriter.CreateDirectory(BuildLocalPathFromWebPath(filePath));

            Debug.WriteLine("Folder created: " + BuildLocalPathFromWebPath(filePath));

            return Content(string.Concat("/", filePath));
        }

        #endregion


        #region DeleteFolder

        [AjaxOnly]
        public ActionResult DeleteFolder(string folderPath)
        {
            Debug.WriteLine("DeleteFolder from request: " + folderPath);

            if (string.IsNullOrWhiteSpace(folderPath)) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            string basePath = BuildLocalPathFromWebPath("Uploads");
            string concatWithBasePath = FileSystemUtilities.ChangePathMinusesToBackSlashes(folderPath);

            Debug.WriteLine("DeleteFolder after parse: " + Path.Combine(basePath, concatWithBasePath));

            if (!Directory.Exists(Path.Combine(basePath, concatWithBasePath))) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            return PartialView(new FileSystemReader(basePath, concatWithBasePath).GetDirectories());
        }

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ValidateAntiForgeryToken]
        public void DoDeletingFolder(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return;

            FileSystemWriter.DeleteDirectoryWithFiles(path);

            Debug.WriteLine("Folder deleted: " + path);
        }

        #endregion


        #region DeleteFolder

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ValidateAntiForgeryToken]
        public void DoDeletingFile(string filenameWithPath)
        {
            if (string.IsNullOrWhiteSpace(filenameWithPath)) return;

            FileSystemWriter.DeleteFile(BuildLocalPathFromWebPath(filenameWithPath));

            Debug.WriteLine("File deleted: " + BuildLocalPathFromWebPath(filenameWithPath));
        }

        #endregion


        #region PostingFilesToServer

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ValidateAntiForgeryToken]
        public void DoPostingFilesToServer(string filePath, string newFileName, HttpPostedFileBase file)
        {
            var fileSystemWebUploader = new FileSystemWebUploader(BuildLocalPathFromWebPath(filePath), file);

            if (!string.IsNullOrWhiteSpace(newFileName) && newFileName != file.FileName)
                fileSystemWebUploader.Upload(newFileName);
            else
                fileSystemWebUploader.Upload();

            Debug.WriteLine("File uploaded: \n" + "New file name: " + newFileName + "\n" + BuildLocalPathFromWebPath(filePath));
        }

        #endregion


        #region RenamingFiles

        [Authorizer(Roles = "Administrators")]
        [HttpPost, ValidateAntiForgeryToken]
        public void DoRenamingFiles(string oldFilenameWithPath, string newFilenameWithPath)
        {
            FileSystemWriter.MoveFile(BuildLocalPathFromWebPath(oldFilenameWithPath), BuildLocalPathFromWebPath(newFilenameWithPath));

            Debug.WriteLine("File renamed: \n" + "Old path: " + BuildLocalPathFromWebPath(oldFilenameWithPath) + "\nNew path: " + BuildLocalPathFromWebPath(newFilenameWithPath));
        }

        #endregion


        #region GettingFilesFromServer

        public ActionResult GetFilesFromServer(string webFilePath)
        {
            var fileSystemReader = new FileSystemReader(BuildLocalPathFromWebPath(webFilePath));

            var model = new List<FileManagerFileForPreviewViewModel>();

            foreach (IFile file in fileSystemReader.GetFiles())
            {
                model.Add(new FileManagerFileForPreviewViewModel
                {
                    WebPath = string.Concat("/Content", webFilePath),
                    FileName = file.Name,
                    MimeType = MimeMapping.GetMimeMapping(Path.Combine(file.Path, file.Name)),
                    Extension = file.Extension,
                    Size = file.Size
                });
            }

            // Json ActionResult uses JavaScriptSerializer with the same limitation
            // Use Content result and JsonConvert.SerializeObject so we control the serialization.
            return Content(JsonConvert.SerializeObject(model, Formatting.Indented,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), "application/json");
        }

        #endregion


        #region Helpers

        private string BuildLocalPathFromWebPath(string pathFromRequest)
        {
            string pathBuilder = "";

            if (!pathFromRequest.StartsWith("/Content", StringComparison.InvariantCultureIgnoreCase))
                pathBuilder += "/Content";

            if (!pathFromRequest.StartsWith("/", StringComparison.InvariantCultureIgnoreCase))
                pathBuilder += "/";

            return Server.MapPath(string.Concat("~", pathBuilder, pathFromRequest));
        }

        #endregion
    }
}