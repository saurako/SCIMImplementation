var sendrequest;

function sendData() {
    var jsonObj = {
        "Id": "null",
        "userName": "user4@contoso.com",
        "displayName": "User Four",
        "active": false
    };
           
    
    var sendDataURL = "http://localhost:12699/api/users/";
    sendrequest = new XMLHttpRequest();
    sendrequest.open("POST", sendDataURL, false);
    sendrequest.setRequestHeader("Content-Type", "application/json");
    sendrequest.onreadystatechange = displaySaved;
    sendrequest.send(JSON.stringify(jsonObj)); //JSON.stringify(jsonObj) 
}

function displaySaved(){
    if(sendrequest.readyState == 4){        
        if(sendrequest.status == 201){ //item was created		
            
        }
        else {
        
        }
    }

}
