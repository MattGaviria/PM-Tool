function loadComments(projectId) {
    $.ajax({
        url: '/ProjectManagement/ProjectComment/GetComments?projectId=' + projectId,
        method: 'GET',
        success: function (data) {
            var commentsHtml = '';
            for (var i = 0; i < data.length; i++) {
                commentsHtml += '<div class="comment">';
                commentsHtml += '<p>' + data[i].content + '</p>';
                commentsHtml += '<span>Posted On: ' + new Date(data[i].datePosted).toLocaleDateString() + '</span>';
                commentsHtml += '</div>';
            }
            $('#commentsList').html(commentsHtml);
        }
    });

    
    
}
$(document).ready(function () {
    
    //load comments
    var projectId = $('#projectComments input[name="ProjectId"]').val();
    
    loadComments(projectId);
    
    // Submit event for new comment
    $('#addCommentForm').submit(function (evt) {
        evt.preventDefault();
        
        var formData = {
            ProjectId: projectId,
            Content: $('#projectComments textarea[name="Content"]').val()
        };

        $.ajax({
            url: '/ProjectManagement/ProjectComment/AddComment',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(formData),
            
            success: function (response) {
                if (response.success) {
                    $('#projectComments textarea[name="Content"]').val('');
                    loadComments(projectId);
                } else {
                    alert(response.message);
                }
            },
            error: function (xhr, status, error) {
                console.error("Error posting comment:", xhr.responseText);
                alert("Error: " + xhr.responseText);
            }
        })
    })
});
