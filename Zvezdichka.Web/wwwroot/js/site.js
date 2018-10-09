// Write your JavaScript code.

$("img").on("error",
    function() {
        $(this).parent().hide();
        $(this).parent().remove();
    });

$(".alert-dismissable").delay(2000).fadeOut(3000);

var checkboxes = $('.product-column').find('input:checkbox');

for (let i = 0; i < checkboxes.length; i++) {
    checkboxes[i].onclick = function() {
        if (checkboxes[i].checked) {
            // console.log($(this).parent());

            $(this).parent().parent().parent().addClass("product-column-selected");
        } else {
            $(this).parent().parent().parent().removeClass("product-column-selected");

        }
    };
}

function addAlert(alertStyle, message) {
    //The required URL parameter specifies the URL you wish to load.
    //The optional data parameter specifies a set of querystring key/value pairs to send along with the request.
    // data -> {string alertStyle, string message, bool dismissable}

    var urlWithQuery = alertURL + '?alertStyle=' + alertStyle + '&message=' + encodeURIComponent(message);
    console.log("alert message: " + message);
    console.log("alert url: " + urlWithQuery);

    $("#alerts-container").load(urlWithQuery,
        function() {
            $(".alert-dismissable").delay(2000).fadeOut(3000);
            $("html, body").animate({ scrollTop: $("body").offset().top }, 1000);
        });
}

function deleteCloudinaryPhoto(e, photoId) {
    var urlPhotoId = encodeURIComponent(photoId);

    var deleteUrl = "/products/home/deletecloudinaryfileasync?name=" + urlPhotoId;

    $.ajax({
            url: deleteUrl,
            method: "DELETE"
        })
        .done(function() {

            $(e).parent().parent().parent().fadeOut(300, function() { $(this).remove(); });

            addAlert("Success", "Image deleted.");
        });
}

function updateProductThumbnailSource(productName, thumbnailNumber) {

    var cloudinaryUrl = $("#cloudinary-image-" + thumbnailNumber).attr("src");
    console.log(cloudinaryUrl);

    $.ajax({
            url: "/api/products",
            method: "PUT",
            data: { productName: productName, newThumbnailSource: cloudinaryUrl }
        })
        .done(function() {
            addAlert("Success", thumbnailUpdateSuccessfulMessage);
        })
        .fail(function() {
            addAlert("Danger", thumbnailUpdateFailedMessage);
        });
}

function addToCart(prodId, quantity) {
    $.ajax(
            {
                type: "POST",
                traditional: true,
                url: "shopping/home/addToCart",
                data: {
                    productId: prodId,
                    quantity: quantity
                }
            })
        .done(function(resp) {
            addAlert("Success", resp);
        })
        .fail(function(resp) {
            console.dir(resp);
            addAlert("danger", resp.responseText);
        });
}

function addCommentOnEnter(productId, username) {
    var key = window.event.keyCode;

    // If the user hasn't pressed enter
    if (key !== 13) {
        return;
    }

    addComment(productId, username);
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
            $("#add-comment-message").val("");
        })
        .fail(function() {
            alert("Error adding comments");
        });
}

function editComment(commentId) {

    var oldComment = $("#comment-" + commentId).find("td.comment-content").text().trim();

    if (document.getElementById("edit-comment-div") !== null) {
        $("#edit-comment-div").parent().fadeOut(300,
            function() {
                $("#edit-comment-div").parent().remove();
            });
        return;
    }

    var testElement = $('<tr style="display: none;">' +
        "</tr>");

    console.dir(testElement);

    var newTextEditor = $(
        '<tr style="display: none;">' +
        "<td>" +
        '<div class="form-group basic-textarea rounded-corners" id="edit-comment-div">' +
        '<label for="edit-comment">Message: </label>' +
        '<textarea onkeypress="sendEditAjax(' +
        commentId +
        ');" class="form-control z-depth-1 comment-message" id="edit-comment-message" rows="3" placeholder="Write something here...">' +
        oldComment +
        "</textarea>" +
        "</div>" +
        "</td>" +
        "</tr>");

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
            addAlert('info', 'Comment edited.');

            //remove editor
            $("#edit-comment-div").parent().fadeOut(300,
                function() {
                    $("#edit-comment-div").parent().remove();
                });

            //update comment field
            $("#comment-" + commentId).find("td.comment-content").html(newMessage);
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

    console.log("change thumbnail on hover");
    $(".thumbnail-image").hover(
        function() {
            $(this).addClass("thumbnail-image-hovered");
            $("#main-image").attr("src", $(this).attr("src"));
        },
        function() {
            $(this).removeClass("thumbnail-image-hovered");
        }
    );
}

function allItemsSelectorListener() {


    var productColumns = $('.product-column');
    productColumns.each(function(index) {
        if ($('#all-items-selector').prop('checked') === true) {
            changeItemSelector(jQuery(this), true);
        } else {
            changeItemSelector(jQuery(this), false);
        }
    });

}


function changeItemSelector(item, changeTo) {
    if (changeTo === true) {
        jQuery(item).addClass("product-column-selected");
        item.find('input:checkbox:first').prop('checked', true);
    } else {
        jQuery(item).removeClass("product-column-selected");
        item.find('input:checkbox:first').prop('checked', false);
    }
}


function getDateTimeLocal() {
    var now = new Date();

    var month = now.getMonth() + 1;
    if (month < 10) {
        month = '0' + month;
    }

    var date = now.getDate();
    if (date < 10) {
        date = '0' + month;
    }

    var hours = now.getHours();
    if (hours < 10) {
        hours = '0' + hours;
    }

    var minutes = now.getMinutes();
    if (minutes < 10) {
        minutes = '0' + minutes;
    }

    var format = now.getFullYear() +
        '-' +
        month +
        '-' +
        date +
        'T' +
        hours +
        ':' +
        minutes;

    return format;
}