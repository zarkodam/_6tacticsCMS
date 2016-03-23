/*------------------------------------*/
//	Utilis - common.js
/*------------------------------------*/

(function ($, undefined) {

	$.fn.getAttributes = function (prefix) {
		var el = this[0];
		var result = null;
		if (!!el) {
			var searchAttr = String.format('{0}-', prefix);
			var re = new RegExp(searchAttr, 'i');
			var attrs = _.chain(el.attributes)
				.filter(function (x) {
					return x.name.search(re) == 0;
				})
				.map(function (x) {
					var isString = x.value.match(/^true$|^false$|^\d+[\.]?\d+$/) == null;
					var name = _.last(x.name.split(re));
					var formatValue = ' "{0}": "{1}" ';
					if (!isString)
						formatValue = ' "{0}": {1} ';
					return String.format(formatValue, name, x.value);
				})
				.value();
			if (!!attrs.length) {
				var attrsString = String.format('{{0}}', attrs.join(','));
				result = $.utilis.parseJS(attrsString);
			}
		}
		return result;
	};

	// Forsing render invisible elements to measure size
	$.fn.forceVisible = function (fun) {
		var hidders = this.filter(':hidden').parents().filter(function () {
			return $(this).css('display') == 'none';
		});
		hidders.addClass('visible_important');
		fun.call(this);
		hidders.removeClass('visible_important');

		return this;
	}

	// Add jQuery 'onx' modified 'on' function, execute handler only if event target match selector
	$.fn.onx = function () {
		var selector = null;
		var eventType = null;
		var handler = null;
		var func = function (e) {
			if (e.currentTarget != e.target)
				return;
			handler.apply(this, arguments);
		};
		if (_.isFunction(arguments[1])) {
			eventType = arguments[0];
			handler = arguments[1];
			this.on(eventType, func);
		} else {
			selector = arguments[0];
			eventType = arguments[1];
			handler = arguments[2];
			this.on(selector, eventType, func);
		}
	};

	// Avoid console errors in browsers that lack a console.
	// https://github.com/h5bp/html5-boilerplate/blob/master/js/plugins.js
	var method;
	var noop = function () { };
	var methods = ['assert', 'clear', 'count', 'debug', 'dir', 'dirxml', 'error', 'exception', 'group', 'groupCollapsed', 'groupEnd', 'info', 'log', 'markTimeline', 'profile', 'profileEnd', 'table', 'time', 'timeEnd', 'timeStamp', 'trace', 'warn'];
	var length = methods.length;
	var console = (window.console = window.console || {});
	while (length--) {
		method = methods[length];
		// Only stub undefined methods.
		if (!console[method]) {
			console[method] = noop;
		}
	}

	if (window.ko) {
		// knockout custom bindings
		var MS_JSON_DATE_REGEX = /\/(Date\(-?\d*\))\/$/;
		var JSON_DOT_NET_REGEX = /^[\d]{4}-[\d]{2}-[\d]{2}T[\d]{2}:[\d]{2}:[\d]{2}$/;
		var updateObservableFormatted = function (element, valueAccessor, allBindingsAccessor, viewModel) {
			var allBindings = allBindingsAccessor();
			var format = allBindings.format;
			var value = ko.utils.unwrapObservable(valueAccessor());
			var updateValue = value;
			// Check if date
			if (_.isString(value)) {
				// Match MS date format
				var matchMSDate = value.match(MS_JSON_DATE_REGEX);
				if (matchMSDate) {
					updateValue = eval('new ' + matchMSDate[1]); // Create date
					if (!format)
						format = 'd';
				} else {
					// Match JSON.NET date fromat
					var matchDate = value.match(JSON_DOT_NET_REGEX);
					if (matchDate) {
						updateValue = new Date(value); // Create date
						if (!format)
							format = 'd';
					}
				}
			}
			if (!!format) {
				updateValue = Globalize.format(updateValue, format);
			}
			updateValue = updateValue == null ? '' : updateValue;

			if (!!allBindings.value) {
				$(element).val(updateValue);
			}
			else {
				$(element).text(updateValue);
			}
		};
		ko.bindingHandlers.text.update = updateObservableFormatted;
		ko.bindingHandlers.value.update = updateObservableFormatted;
		// knockout helpers
		ko.bindingHandlers.selected = {
			init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
				$(element).bind('blur', function (e) {
					valueAccessor()(false);
				});
			},
			update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
				var selected = ko.utils.unwrapObservable(valueAccessor());
				if (!!selected && !$(element).is(':focus'))
					$(element).select().focus();
			}
		};
		ko.bindingHandlers.datepicker = {
			init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
				var options = allBindingsAccessor().datepicker || {};
				$(element).datepicker(options);
			}
		};
		ko.bindingHandlers.datetimepicker = {
			init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
				var options = {
					dateFormat: String.format('{0} 00:00', $.datepicker._defaults.dateFormat)
				};
				$.extend(options, allBindingsAccessor().datetimepicker || {});
				$(element).datepicker(options);
			}
		};
		ko.bindingHandlers.buttonset = {
			init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
				var options = allBindingsAccessor().buttonset || {};
				$(element).buttonset(options);
			}
		};
		ko.format = function (format) {
			var args = _.chain(arguments).tail().map(function (n) {
				return ko.toJS(n);
			}).value();
			return String.format.apply(this, [format].concat(args));
		},
		ko.bindingHandlers.slideVisible = {
			init: function (element, valueAccessor) {
				// Initially set the element to be instantly visible/hidden depending on the value
				var value = valueAccessor();
				$(element).toggle(ko.utils.unwrapObservable(value));
			},
			update: function (element, valueAccessor) {
				// Whenever the value subsequently changes, slowly fade the element in or out
				var value = valueAccessor();
				ko.utils.unwrapObservable(value) ? $(element).slideDown() : $(element).slideUp();
			}
		};
	}
})(jQuery);


