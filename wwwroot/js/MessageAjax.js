
function appendMessage(content, isMyMessage) {
    const messageContainer = document.getElementById('divmsg');
    const messageElement = document.createElement('div');
    messageElement.textContent = content;
    messageElement.classList.add('message');

    if (isMyMessage) {
        messageElement.classList.add('my-message');
    } else {
        messageElement.classList.add('friend-message');
        
    }

    messageContainer.appendChild(messageElement);
    messageContainer.scrollTop = messageContainer.scrollHeight; 
}

$(document).ready(function () {  

    $("#btnsend").click(function () {  
        send();  
    });  

    getmyfriends(); 

    
    setInterval(function () {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: '{}',
            url: "/Home/receive",
            dataType: "json",
            success: function (response) {
                if (response != null) {
                    appendMessage("Friend: " + response, false);
                }
            }
        });
    }, 1000);

     
});

function getmyfriends()  
    {  
        $.ajax({  
            type: "POST",  
            url: "/Home/friendlist",  
            data: '{}',  
            dataType: "json",  
            success: function (r) {  
                var FriendList = $("#FriendList");  
                FriendList.empty().append('<option selected="selected" value="0">select</option>');  
                for (var i = 0; i <r.length; i++) {  
                    FriendList.append($("<option></option>").val(r[i]).html(r[i]));  
                }   
            },  
            error: function (r) {  
                alert("error");  
            }  
        });  
    }  

function send() {  
    var message = $("#txtmsg").val();  
    var friend = $("#FriendList").val();  
    var data = $("#divmsg").html();  
    $("#divmsg").html(data + "<br>Me:" + message);  
    $("#txtmsg").val("");  
    console.log("To: "+friend);
    console.log("Message: "+message);
    $.ajax({  
        type: "POST",   
        data: {"message":message,"friend":friend},  
        url: "/Home/sendmsg",  
        dataType: "json",  
        success: function (response) {  
            var data = $("#divmsg").html();  
            if(message == ""){
                $("#divmsg").html(data + "<br>Me :" + message);
            }  
        } 
    });  
} 