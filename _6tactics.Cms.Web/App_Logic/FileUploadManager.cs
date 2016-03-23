using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace _6tactics.Cms.Web.App_Logic
{
	public class FileUploadManager
	{
		#region Constants

		public const string UPLOAD_CACHE_ROOT_FOLDER = "~/App_Data/UploadsCache";
		private readonly TimeSpan MAX_FILE_CACHE_DURATION = TimeSpan.FromHours(4);

		#endregion

		#region Properties

		public string UploadCacheRootPath { get; private set; }

		// Not thread safe, not enough time
		public static bool CleanUpInProgress { get; private set; }

		#endregion

		#region Constructors

		public FileUploadManager(Func<string, string> mapPathMethod)
		{
			this.UploadCacheRootPath = mapPathMethod(UPLOAD_CACHE_ROOT_FOLDER);
		}

		#endregion

		#region Public methods

		public Action<string> SaveFileChunk(string uuid, int chunk, int chunks, HttpPostedFileBase postedFile)
		{
			// Upload cache paths
			var uuidFolder = this.GetUploadCacheFilePath(uuid, true);
			var filePath = Path.Combine(uuidFolder.FullName, chunk.ToString());

			// Save file
			postedFile.SaveAs(filePath);

			Action<string> saveWholeFile = null;

			if ((chunk + 1) == chunks)
			{
				saveWholeFile = wholeFilePath =>
				{
					// Combine whole file
					using (var fs = new FileStream(wholeFilePath, FileMode.Create))
					{
						var partsPaths =
							Enumerable
								.Range(0, chunks)
								.Select(x => Path.Combine(uuidFolder.FullName, x.ToString()));

						foreach (var path in partsPaths)
							using (var fsPart = new FileStream(path, FileMode.Open, FileAccess.Read))
								fsPart.CopyTo(fs);
					}

					// Delete parts
					uuidFolder.Delete(true);
				};
			}

			return saveWholeFile;
		}

		public void CleanUp()
		{
			// Check if clean up is not already started and upload cache folder exist
			if (!FileUploadManager.CleanUpInProgress && Directory.Exists(this.UploadCacheRootPath))
			{
				try
				{
					FileUploadManager.CleanUpInProgress = true;

					// Find old folders
					var now = DateTime.Now;
					var folders = Directory.EnumerateDirectories(this.UploadCacheRootPath)
						.Where(x => (now - Directory.GetLastWriteTime(x)) > this.MAX_FILE_CACHE_DURATION);

					foreach (var folder in folders)
					{
						try
						{
							Directory.Delete(folder, true);
						}
						catch (Exception ex)
						{
							// Log exception
							var ctx = HttpContext.Current;
							Trace.WriteLine(ex.Message);
							//Elmah.ErrorSignal.FromContext(ctx).Raise(ex);
						}
					}
				}
				finally
				{
					FileUploadManager.CleanUpInProgress = false;
				}
			}
		}

		#endregion

		#region Helpers

		private DirectoryInfo GetUploadCacheFilePath(string uuid, bool createIfNotExist = false)
		{
			var uuidPath = Path.Combine(this.UploadCacheRootPath, uuid);
			var uuidFolder = new DirectoryInfo(uuidPath);
			if (createIfNotExist && !uuidFolder.Exists)
			{
				uuidFolder.Create();
			}
			return uuidFolder;
		}

		#endregion
	}
}