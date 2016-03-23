/*------------------------------------*/
//	Utilis - utilis.js
/*------------------------------------*/

(function (utilis, $, undefined) {

	var dataFormValidationGroupKey = '__dataFormValidation__';

	$.extend(utilis, {
		// Init jquery plugin
		// $.fn.<plugin name> = function (options) {
		//		return $.utilis.pluginInit.call(this, 'data-<name>', <control class>, options, $.fn.<plugin name>.defaults);
		// };
		pluginInit: function (name, control, options, defaults) {
			var dataAttrOptions = String.format('data-{0}-options', name);
			var init = function (el, opt) {
				var $el = $(el);
				var controlInstance = $el.data(name);
				if (!controlInstance || !(controlInstance instanceof control)) {
					var attrOptions = $el.getAttributes(dataAttrOptions);
					var attrObjectOptions = utilis.parseJS($el.attr(dataAttrOptions));
					var mergedOptions = $.extend(true, {}, defaults, attrOptions, attrObjectOptions, opt);
					controlInstance = new control(el, mergedOptions);
					$el.data(name, controlInstance);
				}
				return controlInstance;
			};
			if (typeof (options) == 'string' && options == 'control') {
				var controls = this.map(function (i, n) { return init(n, null); });
				return controls[0];
			} else {
				var controls = this.map(function (i, n) { return init(n, options); });
			}
			return this;
		},
		addValidationGroupSupport: function () {
			var orginalCheck = $.validator.prototype.check;
			$.extend(true, $.validator, {
				prototype: {
					// extend jquery validation check function
					check: function (element) {
						// Find validation group
						var $form = $(this.currentForm);
						var info = $form.data(dataFormValidationGroupKey);
						var validSelector = ':not([data-val-group])';
						if (!!info && !!info.group) {
							// Validate group only
							validSelector = String.format('[data-val-group~={0}]', info.group);
						}
						if (!$(element).is(validSelector)) {
							this.successList.push(element);
							return true;
						}
						return orginalCheck.call(this, element);
					}
				}
			});

			// Save validation group
			var onSubmit = function (e) {
				var $form = $(this).parents('form:first');
				var valGroup = $(this).attr('data-val-group');
				var info = { group: valGroup };
				$form.data(dataFormValidationGroupKey, info);
				$.validator.unobtrusive.parse($form);
			};
			$(document).on('click', 'form :submit', onSubmit);
			$(document).on('change', 'form [data-u-autopostback="true"][data-val-group]', onSubmit);
		},
		// Unobtrusive: add 'submitCommand' and 'submitArgument' to post data
		// input[type=submit][data-u-button-command]
		// input[type=submit][data-u-button-argument]
		addPostCommandSupport: function () {
			var onSubmit = function (e) {
				var btn = $(this);
				var command = btn.attr('data-u-button-command');
				var argument = btn.attr('data-u-button-argument');

				// Find/create hidden field
				var form = btn.parents('form:first');
				var addRemoveHidden = function (hiddenName, value) {
					var inputArg = form.find(String.format(':hidden[name={0}]', hiddenName));
					if (!inputArg.length)
						inputArg = $('<input type="hidden" />').attr('name', hiddenName).insertAfter(btn);
					inputArg.val(value);
				};

				if (!!command) {
					addRemoveHidden('submitCommand', command);
				}
				if (!!argument) {
					addRemoveHidden('submitArgument', argument);
				}
				if (!!e.data && !!e.data.submit)
					form.submit();
			};

			$(document).on('click', 'form :submit[data-u-button-command], form :submit[data-u-button-argument]', onSubmit);
			$(document).on('change', 'form [data-u-autopostback="true"]', { submit: true }, onSubmit);
		},
		// Math.round(<number>, <decimals>)
		addBankersRoundingSupport: function () {
			var orginalRound = Math.round;
			function evenRound(num, decimalPlaces) {
				var result = null;
				if (decimalPlaces == null) {
					result = orginalRound(num);
				} else {
					var d = decimalPlaces || 0;
					var m = Math.pow(10, d);
					var n = +(d ? num * m : num).toFixed(8); // Avoid rounding errors
					var i = Math.floor(n), f = n - i;
					var r = (f == 0.5) ? ((i % 2 == 0) ? i : i + 1) : orginalRound(n);
					result = d ? r / m : r;
				}
				return result;
			}
			Math.round = evenRound;
		},
		// Url helpers
		// Get url info(search & hash values)
		parseUrl: function (url) {
			var parseObj = function (str) {
				var r = null, re = /([^&\?#=]+)=([^&]*)/g, m;
				if (str) {
					r = {};
					var s = str[0];
					while (m = re.exec(s)) {
						r[m[1]] = m[2];
					}
				}
				return r;
			}
			var urlDecoded = decodeURIComponent(url || '');
			var hostPathMatch = urlDecoded.match(/[^\?#]+/);
			var hostPath = hostPathMatch != null ? hostPathMatch[0] : null;
			var searchMatch = urlDecoded.match(/(?=\?)[^#]+/);
			var hashMatch = urlDecoded.match(/(?=#).+/);
			return {
				search: parseObj(searchMatch),
				hash: parseObj(hashMatch),
				path: hostPath,
				combine: function () {
					var r = '';
					if (this.path)
						r = r + this.path;
					if (this.search)
						r = r + '?' + $.param(this.search, true);
					if (this.hash)
						r = r + '#' + $.param(this.hash);
					return r;
				}
			};
		},
		// Parse string as javascript
		parseJS: function (JSstring) {
			var result = null;
			try {
				result = eval(String.format('({0})', JSstring));
			} catch (ex) { }
			return result;
		},
		// Get property by name dot separated
		getObject: function (obj, name) {
			var names = name.split('.');
			$.each(names, function (i, n) {
				obj = obj[n];
				if (obj == null)
					return false; // break
			});
			return obj;
		},
		/**
		Formats the specified number as a size string for example 1024 becomes 1 KB.
		
		@method formatSize
		@static
		@param {Number} size Size to format as string.
		@return {String} Formatted size string.
		*/
		formatSize: function (size) {
			if (size === undefined || /\D/.test(size)) {
				return 'N/A';
			}
			// TB
			if (size > 1099511627776) {
				return Math.round(size / 1099511627776, 1) + " " + 'TB';
			}
			// GB
			if (size > 1073741824) {
				return Math.round(size / 1073741824, 1) + " " + 'GB';
			}
			// MB
			if (size > 1048576) {
				return Math.round(size / 1048576, 1) + " " + 'MB';
			}
			// KB
			if (size > 1024) {
				return Math.round(size / 1024, 1) + " " + 'KB';
			}
			return size + " " + 'b';
		},
		/**
		Parses the specified size string into a byte value. For example 10kb becomes 10240.
	
		@method parseSizeStr
		@static
		@param {String/Number} size String to parse or number to just pass through.
		@return {Number} Size in bytes.
		*/
		parseSize: function (size) {
			if (typeof (size) !== 'string') {
				return size;
			}
			var muls = {
				t: 1099511627776,
				g: 1073741824,
				m: 1048576,
				k: 1024
			},
				mul;
			size = /^([0-9]+)([mgk]?)$/.exec(size.toLowerCase().replace(/[^0-9mkg]/g, ''));
			mul = size[2];
			size = +size[1];

			if (muls.hasOwnProperty(mul)) {
				size *= muls[mul];
			}
			return size;
		}
	});
})(jQuery.utilis = jQuery.utilis || {}, jQuery);