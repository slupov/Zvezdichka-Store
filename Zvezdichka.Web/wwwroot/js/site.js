// Write your JavaScript code.
function addToCart(productName) {
    $.ajax(
            {
                type: "GET",
                traditional: true,
                url: "Shopping/Home/AddToCart",
                data: {
                    title: productName,
                    quantity: $("#bag-quantity").val()
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
    var commentText = $("#add-comment-message").val();

    var newComment = $('<tr style="display: none;"><td>' +
        username +
        "<div><p>Added " +
        "just Now" +
        "</p></div></td>" +
        "<td>" +
        commentText +
        "</td><td></td></tr>");

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
            $("#comments-table tr:first").after(newComment);
            $("html, body").animate({ scrollTop: $("#comments-table").offset().top }, 1000);
            newComment.fadeIn(1500);
            $("#add-comment-message").val('');
        })
        .fail(function() {
            alert("Error adding comments");
        });
}

function editComment(commentId) {

    var oldComment = $("#comment-" + commentId).find('td.comment-content').text().trim();

    if (document.getElementById("edit-comment-div") !== null) {
        $("#edit-comment-div").parent().fadeOut(300,
            function() {
                $("#edit-comment-div").parent().remove();
            });
        return;
    }

    var testElement = $('<tr style="display: none;">' +
        '</tr>');

    console.dir(testElement);

    var newTextEditor = $(
        '<tr style="display: none;">' +
        '<td>' +
        '<div class="form-group basic-textarea rounded-corners" id="edit-comment-div">' +
        '<label for="edit-comment">Message: </label>' +
        '<textarea onkeypress="sendEditAjax(' +
        commentId +
        ');" class="form-control z-depth-1 comment-message" id="edit-comment-message" rows="3" placeholder="Write something here...">' +
        oldComment +
        '</textarea>' +
        '</div>' +
        '</td>' +
        '</tr>');

    console.dir(newTextEditor);
    var commentRow = $("#comment-" + commentId);

    console.log("commentRow");
    console.dir(commentRow);

    commentRow.after(newTextEditor);
    newTextEditor.fadeIn(300);
}

function sendEditAjax(commentId) {
    var key = window.event.keyCode;

    // If the user hasn't pressed enter
    if (key !== 13) {
        return;
    }

    var newMessage = $("#edit-comment-message").val();

    var dataToSend = {
        'id': commentId,
        'Message': newMessage
    };

    $.ajax(
            {
                type: "PUT",
                traditional: true,
                url: "api/comments",
                contentType: "application/json",
                data: JSON.stringify(dataToSend)
            })
        .done(function() {
            alert("Comment edited");

            //remove editor
            $("#edit-comment-div").parent().fadeOut(300,
                function () {
                    $("#edit-comment-div").parent().remove();
                });

            //update comment field
            $("#comment-" + commentId).find('td.comment-content').html(newMessage);
        })
        .fail(function() {
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
                $("#main-image").attr("src", $(this).attr("src"));
            },
            function() {
                $(this).removeClass("thumbnail-image-hovered");
            }
        );
    });
}