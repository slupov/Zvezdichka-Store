// Write your JavaScript code.
function addToCart(productName) {
    $.ajax(
            {
                type: "GET",
                traditional: true,
                url: "Shopping/Home/AddToCart",
                data: {
                    title: productName,
                    quantity: $('#bag-quantity').val()
                }
            })
        .done(function() {
            alert("Value Added");
        })
        .error(function() {
            alert("Error adding to shopping cart");
        });
}

function addComment(productId, username) {
    var commentText = CKEDITOR.instances["add-message-content"].getData();

    var newComment = $('<tr style="display: none;"><td>' +
        username +
        '<div><p>Added ' +
        'just Now' +
        '</p></div></td>' +
        '<td>' +
        commentText +
        '</td><td></td></tr>');

    var dataToSend = {
        "Message": commentText,
        "ProductId": productId,
        "Username": username
    };

    $.ajax(
            {
                type: "POST",
                traditional: true,
                url: "api/comments",
                contentType: "application/json",
                data: JSON.stringify(dataToSend)
            })
        .done(function() {
            $("#comments-table").append(newComment);
            newComment.fadeIn("slow");
        })
        .fail(function() {
            alert("Error adding comments");
        });
}

function editComment(commentId) {

    if (document.getElementById('edit-comment-div') !== null) {
        return;
    }

    var newTextEditor = $('<div class="form-group" id="edit-comment-div">' +
        '<label class="control-label">Message:</label>' +
        '<textarea id="edit-message-content" name="edit-message-content"></textarea>' +
        '</div>');

    var newTextArea = newTextEditor.find('textarea');

    //attach CK editor to the new textarea
    addCkEditor(newTextArea.attr('id'));

    $(".comments-container").prepend(newTextEditor);
    sendEditAjax(commentId);
}

function sendEditAjax(commentId) {
    console.log("sendEditAjax");
    console.dir(CKEDITOR);
    console.dir(CKEDITOR.instances);

//    var commentText = CKEDITOR.instances["edit-message-content"].getData();

    console.log(commentText);

    var dataToSend = {
        'id': commentId,
        'Message': commentText
    };

    $.ajax(
            {
                type: "PUT",
                traditional: true,
                url: "api/comments",
                data: (JSON.stringify(dataToSend))
            })
        .done(function() {
            alert("Comment edited");
        })
        .error(function() {
            alert("Error editing comment");
        });
}

function deleteComment(e, commentId) {

    $.ajax(
            {
                type: "DELETE",
                traditional: true,
                url: "api/comments",
                data: ({ 'id': commentId })
            })
        .done(function() {
            $(e).parent().parent().parent().fadeOut(300, function() { $(this).remove(); });
        })
        .fail(function() {
            alert("Error deleting comment");
        });
}

//a function that changes the main image on hove of some of the thumbnails
function changeThumbnailOnHover() {
    $(document).ready(function() {
        $(".thumbnail-image").hover(
            function() {
                $(this).addClass("thumbnail-image-hovered");
                $("#main-image").attr("src", $(this).attr('src'));
            },
            function() {
                $(this).removeClass("thumbnail-image-hovered");
            }
        );
    });
}