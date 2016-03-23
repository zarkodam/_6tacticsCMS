(function ($) {

    $.duplicatedIdHandler = function (elementNameClassOrIdToCheck, defaultIdName, applyNewId, returnNewId) {
        var idBuffer = [];
        var generatedId = defaultIdName;

        $(elementNameClassOrIdToCheck).each(function () {
            var currentId = $(this).attr('id');

            var isCurrentIdContainsDefaultIdName = currentId.toLowerCase().indexOf(defaultIdName.toLowerCase()) >= 0;

            if (!isCurrentIdContainsDefaultIdName) return true;

            //console.log('defaultIdName: ' + defaultIdName);
            //console.log('currentId: ' + currentId);

            var idsBySameName = idBuffer.filter(function (item) {
                return item === currentId;
            });

            //console.log(idsBySameName);

            if (idsBySameName.length > 0) {
                var idCountNumber = currentId.match(/\d/g);
                var elementNumber = parseInt((idCountNumber != null ? idCountNumber : 0)) + idsBySameName.length;

                generatedId = defaultIdName + "-" + elementNumber;

                if (applyNewId)
                    $(this).attr('id', generatedId);
            }

            idBuffer.push($(this).attr('id'));

        });

        if (returnNewId)
            return generatedId;
    }

    $.fixDuplicatedId = function (elementNameClassOrIdToCheck, defaultIdName) {
        $.duplicatedIdHandler(elementNameClassOrIdToCheck, defaultIdName, true, false);
    }

    $.returnFixedDuplicatedId = function (elementNameClassOrIdToCheck, defaultIdName) {
        return $.duplicatedIdHandler(elementNameClassOrIdToCheck, defaultIdName, false, true);
    }

    $.fixDuplicatedIdAndReturnFixedId = function (elementNameClassOrIdToCheck, defaultIdName) {
        return $.duplicatedIdHandler(elementNameClassOrIdToCheck, defaultIdName, true, true);
    }

})(jQuery);
