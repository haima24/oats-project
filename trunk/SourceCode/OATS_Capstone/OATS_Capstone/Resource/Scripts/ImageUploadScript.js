﻿$(function () {

    var dropbox = $('#dropbox'),
		message = $('.message', dropbox);

    dropbox.filedrop({
        url: '/Tests/UploadFiles',
        paramname: 'files',
        maxFiles: 10,
        maxFileSize: 4,
        allowedfiletypes: ['image/jpeg', 'image/png', 'image/gif'],
        uploadFinished: function (i, file, response) {
            $.data(file).addClass('done');
            // response is the JSON object that controller returns
        },

        error: function (err, file) {
            switch (err) {
                case 'BrowserNotSupported':
                    showMessage('Your browser does not support HTML5 file uploads!');
                    break;
                case 'TooManyFiles':
                    alert('Too many files! Please select 10 at most! (configurable)');
                    break;
                case 'FileTooLarge':
                    alert(file.name + ' is too large! Please upload files up to 4mb (configurable).');
                    break;
                case 'FileTypeNotAllowed':
                    alert(file.name + ' is not supported. You can only upload files with .gif .png and .jpg extension');
                    break;
                default:
                    break;
            }
        },

        uploadStarted: function (i, file, len) {
            createImage(file);
        },

        progressUpdated: function (i, file, progress) {
            $.data(file).find('.progress').width(progress);
        }

    });

    var template = '<div class="preview">' +
			'<span class="imageHolder">' +
			    '<img />' +
			    '<span class="uploaded"></span>' +
			'</span>' +
			'<div class="progressHolder">' +
				'<div class="progress"></div>' +
			'</div>' +
                      '<div class="remove">Remove</div>' +
		     '</div>';

    function createImage(file) {
        var preview = $(template), image = $('img', preview);

        var reader = new FileReader();

        image.width = 100;
        image.height = 100;

        reader.onload = function (e) {

            // e.target.result holds the DataURL which
            // can be used as a source of the image:

            image.attr('src', e.target.result);
        };

        // Reading the file as a DataURL. When finished,
        // this will trigger the onload function above:
        reader.readAsDataURL(file);

        message.hide();
        preview.appendTo(dropbox);

        // Associating a preview container
        // with the file, using jQuery's $.data():

        $.data(file, preview);
    }

    function showMessage(msg) {
        message.html(msg);
    }
});
