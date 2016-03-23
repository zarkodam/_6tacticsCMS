/**
 * @license Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
	config.resize_minWidth = 380;
	config.resize_minHeight = 300;

	config.allowedContent = true;

	//config.skin = 'minimalist';

	config.removePlugins = 'sourcearea';
	config.extraPlugins = 'sourcedialog';

	config.toolbarGroups = [
		{ name: 'clipboard', groups: ['cut', 'copy', 'paste', '', 'clipboard', 'undo'] },
		{ name: 'editing', groups: ['find', 'selection', 'spellchecker', 'editing'] },
		{ name: 'links', groups: ['links'] },
		{ name: 'insert', groups: ['insert'] },
		{ name: 'tools', groups: ['tools'] },
		{ name: 'document', groups: ['mode', 'doctools'] },
		{ name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
		{ name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi', 'paragraph'] },
		{ name: 'styles', groups: ['styles'] },
		{ name: 'colors', groups: ['colors'] },
		{ name: 'others', groups: ['others'] },
	];

	config.removeButtons = 'Save,NewPage,Preview,Print,Templates,Form,Checkbox,Radio,TextField,Textarea,Select,Button,ImageButton,HiddenField,' +
		'Language,Image,Flash,Smiley,Anchor,HorizontalRule,PageBreak,Iframe,Code,document,UIColor,Markdown,gg,Maximize';
};

// List installed plugins(same names for toolbar buttons
//console.log(CKEDITOR.config['plugins'])

CKEDITOR.on('dialogDefinition', function (ev) {
	// Forbid dialog resize
	ev.data.definition.resizable = CKEDITOR.DIALOG_RESIZE_NONE;

	// Take the dialog name and its definition from the event data
	var dialogName = ev.data.name;
	var dialogDefinition = ev.data.definition;

	// Default settings for table plugin
	if (dialogName == 'table') {
		// Get a reference to the "Table Info" tab.
		var infoTab = dialogDefinition.getContents('info');
		//console.log(infoTab);
		//console.log(infoTab['_']['dialog']['_']);

		// Cell spaceing
		txtCellSpace = infoTab.get('txtCellSpace');
		txtCellSpace['default'] = 0;

		// Cell padding
		txtCellPad = infoTab.get('txtCellPad');
		txtCellPad['default'] = 0;

		// Width
		txtWidth = infoTab.get('txtWidth');
		txtWidth['default'] = '100%';

		// Headers
		selHeaders = infoTab.get('selHeaders');
		selHeaders['default'] = 'row';

		// Border
		txtBorder = infoTab.get('txtBorder');
		txtBorder['default'] = 0;

		var advancedTab = dialogDefinition.getContents('advanced');

		// Stylesheet Classes
		advCSSClasses = advancedTab.get('advCSSClasses');
		advCSSClasses['default'] = 'table';
	}
});

// Clearing cache with new timestamp (add after including ckeditor.js script)
//CKEDITOR.timestamp = Date.now();

