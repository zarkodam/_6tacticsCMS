/*------------------------------------*/
//	Utilis - utilis.Class.js
/*------------------------------------*/

(function (utilis, $, undefined) {
	var extend = function(baseType, type) {
		// Temp with empty constructor
		function tmpPrototype() { };
		// Assign prototype by reference
		tmpPrototype.prototype = baseType.prototype;
		// New instance of temp class as prototype for new class
		// That why parameterless constructor
		var newProptotype = new tmpPrototype;
		// Copy new properties to new prototype
		$.each(type.prototype, function (key, value) {
			if (_.isFunction(baseType.prototype[key])) {
				// Base function exist
				newProptotype[key] = function () {
					// Reference this._super to base class method
					// Only for this call
					var tmpSuper = this._super;
					this._super = baseType.prototype[key];
					// Execute method
					var result = value.apply(this, arguments);
					// return existing this._super reference
					this._super = tmpSuper;
					return result;
				}
			} else {
				newProptotype[key] = value;
			}
		});
		// Create new type constructor
		var tmpType = function () {
			this._super = baseType;
			type.apply(this, arguments);
			this._super = null;
		};
		tmpType.setInherit = function (extendingType) {
			return extend(tmpType, extendingType);
		};
		tmpType.prototype = newProptotype;
		tmpType.prototype.constructor = type;

		return tmpType;
	}

	// Utilis base class
	function Class() { };

	utilis.Class = extend(function () { }, Class);

	// String format
	String.format = function () {
		// The string containing the format items (e.g. "{0}")
		// will and always has to be the first argument.
		var theString = arguments[0];

		// start with the second argument (i = 1)
		for (var i = 1; i < arguments.length; i++) {
			// "gm" = RegEx options for Global search (more than one instance)
			// and for Multiline search
			var regEx = new RegExp("\\{" + (i - 1) + "\\}", "gm");
			theString = theString.replace(regEx, arguments[i]);
		}

		return theString;
	};

})(window.utilis = window.utilis || {}, jQuery);