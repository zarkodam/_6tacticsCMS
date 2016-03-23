/*------------------------------------*/
//	Utilis - jquery.upload.js
/*------------------------------------*/

(function ($) {

	var fileMapping = {
		key: function(data) {
			return ko.utils.unwrapObservable(data.uuid);
		},
		create: function (options) {
			var d = ko.mapping.fromJS(options.data);
			d.displayName = ko.computed(function () {
				return String.format('{0} ({1})', d.name(), $.utilis.formatSize(d.size()));
			}, this);
			return d;
		}
	};

	var control = function (element, options) {
		this.options = options;
		this._element = $(element);
		this.selectButton = this._element.find('.upload-button');

		this._init();
	};

	control.prototype = {
		_init: function () {
			// Check for the various File API support.
			// Detect support for Blob slicing (required for chunked uploads)
			if (window.File && window.FileReader && window.FileList && window.Blob && Blob.prototype.slice) {
				// Great success! All the File APIs are supported.
				// Using HTML5 input type file with XMLHttpRequest
				this.fileUpload = new utilis.UploadHtml5(this, this.options);
				this.fileUpload.fixSelectButton(this.selectButton);
			} else {
				if (window.console)
					console.log('The File APIs are not fully supported in this browser, fallback to Silverlight plugin.');
				// Fallback to Silverlight plugin
				this.fileUpload = new utilis.UploadSilverlight(this, this.options);
				this.fileUpload.fixSelectButton(this.selectButton);
			}

			// Event handlers
			$(this.fileUpload)
				.on('added.upload.utilis', $.proxy(this._fileAdded, this))
				.on('started.upload.utilis', $.proxy(this._fileUploadStarted, this))
				.on('progress.upload.utilis', $.proxy(this._fileUploadProgress, this))
				.on('done.upload.utilis', $.proxy(this._fileUploadDone, this))
				.on('error.upload.utilis', $.proxy(this._fileUploadError, this));

			this.model = {
				files: ko.observableArray(),
				showUploadButton: ko.observable(false),
				startClick: $.proxy(this.fileUpload.startAll, this.fileUpload),
				error: ko.observable(),
				abortAll: $.proxy(this.fileUpload.abortAll, this.fileUpload)
			};
			ko.applyBindings(this.model, this._element.get(0));

			// Handle dialog close
			this._element.parents(':ui-dialog')
				.on('dialogclose', $.proxy(function (e) {
					// Abort upload
					this.fileUpload.abortAll();
				}, this));
		},
		_fileAdded: function (e, file) {
			// Add files to model
			this.model.files.removeAll();
			this.model.files.push({
				file: ko.mapping.fromJS(file, fileMapping)
			});
			this.model.showUploadButton(true);
			this.model.error(null);
		},
		_fileUploadStarted: function (e, file) {
			// Hide start upload button
			this.model.showUploadButton(false);
			this.model.error(null);
		},
		_fileUploadProgress: function (e, file) {
			this._updateModel(file);
		},
		_fileUploadDone: function (e, file) {
			// Clear queue
			this.model.files.removeAll();
			// CUSTOM - Alstom: Update file name
			var fileDownload = this._element.parents('[data-ajax-update=true]').find('.u-download');
			var fileName = fileDownload.find('.u-download-name');
			fileName.text(file.response.name);
			fileDownload.show();
		},
		_fileUploadError: function (e, file) {
			var msg = this.options.error_http;
			if (!!file.response) {
				msg = file.response.Message;
			}
			// Show error
			this.model.error(msg);
		},
		_updateModel: function (file) {
			var f = _.find(this.model.files(), function (x) {
				var uuid = ko.utils.unwrapObservable(x.file.uuid);
				return uuid == file.uuid;
			});
			if (!!f) {
				ko.mapping.fromJS(file, fileMapping, f.file);
			}
		}
	};
	$.fn.upload = function (options) {
		return $.utilis.pluginInit.call(this, 'u-upload', control, options, $.fn.upload.defaults);
	};

	$.fn.upload.defaults = {
		url: null,
		//max_file_size: '4Gb', Not implemented
		chunk_size: '4Mb',
		multi_selection: false,
		error_http: 'Http error',
		error_file_size: 'Selected file is to large',
		silverlight_xap: '/Scripts/utilis/upload/FileUpload.xap'
	};

})(jQuery);
