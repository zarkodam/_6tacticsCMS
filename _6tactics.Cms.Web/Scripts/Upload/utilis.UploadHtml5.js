/*------------------------------------*/
//	Utilis - utilis.UploadHtml5.js
/*------------------------------------*/

(function (utilis, $, undefined) {

	var defaults = {
		url: null,
		chunk_size: '1Mb',
		multi_selection: false
	};

	var STOPED = 'STOPED';
	var UPLOADING = 'UPLOADING';
	var DONE = 'DONE';
	var ERROR = 'ERROR';

	function UploadHtml5(client, options) {
		if (!this.options) {
			this.options = $.extend(true, {}, defaults, options);
		}
		this.client = client;
		this._init();
	};

	UploadHtml5.prototype = {
		// private methods
		_init: function () {
			this._initQueue();
			// Set file upload event handler
			this.client._element
				.on('change', 'input[type="file"]', $.proxy(this._onSelectedFiles, this));
		},
		_initQueue: function () {
			this.queue = [];
		},
		_onSelectedFiles: function (e) {
			// Abort all existing in queue
			this.abortAll();
			// Clear queue
			this._initQueue();
			// Add selected files to queue
			var files = e.target.files || e.dataTransfer.files;
			$.each(files, $.proxy(function (i, file) {
				this._addToQueue(file);
			}, this));
			// Replace input file
			var fileInput = $(e.target);
			var clone = fileInput.clone();
			fileInput.replaceWith(clone);
		},
		_addToQueue: function (file) {
			// Check file size limit
			// file.size <= 300000000
			var chunkSize = $.utilis.parseSize(this.options.chunk_size);
			var chunks = Math.ceil(file.size / chunkSize);
			var fileQueue = {
				uuid: this._uuid(),
				status: STOPED,
				file: file,
				chunks: chunks,
				chunk: 0,
				position: 0,
				uploaded: 0
			};
			this.queue.push(fileQueue);
			// Trigger event
			$(this).trigger('added.upload.utilis', this._createFileStatus(fileQueue));

			return fileQueue;
		},
		_sendBlob: function (fileQueue) {
			var file = fileQueue.file;
			var start = fileQueue.position;
			var end = start + $.utilis.parseSize(this.options.chunk_size);
			var blob = file.slice(start, end);
			var formData = new FormData(); // this will submit as a "multipart/form-data" request
			formData.append('file', blob); // 'file' is what the server will call the blob
			formData.append('uuid', fileQueue.uuid); // unique file identifier
			formData.append('name', file.name); // file name
			formData.append('chunk', fileQueue.chunk); // chunk
			formData.append('chunks', fileQueue.chunks); // chunks
			var xhr = new XMLHttpRequest();
			fileQueue.xhr = xhr;
			xhr.open('POST', this.options.url, true);

			//xhr.setRequestHeader("Content-Type", "application/octet-stream");
			xhr.setRequestHeader("Cache-Control", "no-cache");
			xhr.setRequestHeader("X-Requested-With", "XMLHttpRequest");
			//xhr.setRequestHeader("X-File-Name", encodeURI(file.name));
			//xhr.setRequestHeader("X-File-Size", file.size);

			// Listen to the upload ready state changed
			xhr.onreadystatechange = $.proxy(function (e) {
				if (xhr.readyState == XMLHttpRequest.DONE) {
					if (xhr.status == 200) {
						fileQueue.responseText = xhr.response;
						fileQueue.uploaded = fileQueue.position + blob.size;
						// Trigger event
						$(this).trigger('progress.upload.utilis', this._createFileStatus(fileQueue));
						// Status OK, send next chunk
						var nextChunk = fileQueue.chunk + 1;
						if (nextChunk < fileQueue.chunks) {
							fileQueue.chunk = nextChunk;
							fileQueue.position = end;
							this._sendBlob(fileQueue);
						}
						else {
							fileQueue.status = DONE;
							// Trigger event
							$(this).trigger('done.upload.utilis', this._createFileStatus(fileQueue));
						}
					}
					else {
						fileQueue.responseText = xhr.response;
						// Status ERROR
						fileQueue.status = ERROR;
						// Trigger event
						$(this).trigger('error.upload.utilis', this._createFileStatus(fileQueue));
					}
				}
			}, this);
			// Listen to the upload progress
			xhr.upload.onprogress = $.proxy(function (e) {
				if (e.lengthComputable) {
					// Compute uploaded, max file size
					var uploaded = fileQueue.position + e.loaded;
					fileQueue.uploaded = Math.min(uploaded, fileQueue.file.size);
					// Trigger event
					$(this).trigger('progress.upload.utilis', this._createFileStatus(fileQueue));
				}
			}, this);
			fileQueue.status = UPLOADING;
			xhr.send(formData);
		},
		_uuid: function () {
			var guid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
				var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
				return v.toString(16);
			});
			return guid;
		},
		_createFileStatus: function (fq) {
			var response = null;
			try {
				response = JSON.parse(fq.responseText);
			} catch (e) { }
			var percent = (fq.uploaded / fq.file.size) * 100;
			return {
				uuid: fq.uuid,
				name: fq.file.name,
				size: fq.file.size,
				uploaded: fq.uploaded,
				uploadedPercent: percent,
				status: fq.status,
				response: response
			};
		},
		// public methods
		startAll: function () {
			$.each(this.queue, $.proxy(function (i, fq) {
				this.start(fq);
				// Trigger event
				$(this).trigger('started.upload.utilis', this._createFileStatus(fq));
			}, this));
		},
		start: function (fileQueue) {
			if (fileQueue.status != UPLOADING) {
				this._sendBlob(fileQueue);
			}
		},
		abortAll: function () {
			$.each(this.queue, $.proxy(function (i, fq) {
				this.abort(fq);
			}, this));
		},
		abort: function (fileQueue) {
			var xhr = fileQueue.xhr;
			if (!!xhr) {
				fileQueue.status = STOPED;
				xhr.abort();
			}
		},
		fixSelectButton: function (selectButton) {
			var fileInputContainer = $('<span/>').appendTo(selectButton);
			selectButton.css({
				'position': 'relative',
				'padding': 0,
				'overflow': 'hidden',
				'vertical-align': 'top'
			});
			var fileUploadEl = $('<input type="file"/>')
				.attr('multiple', this.options.multi_selection)
				.css({
					'position': 'absolute',
					'left': '0px',
					'top': '0px',
					'height': '100%',
					'width': '100%',
					'opacity': 0.001,
					'z-index': 2000,
					'cursor': 'pointer'
				})
				.appendTo(fileInputContainer);
		}
	};

	// UploadHtml5 class inherit Class
	utilis.UploadHtml5 = utilis.Class.setInherit(UploadHtml5);

})(window.utilis, jQuery);