function setStyleButton(sender, mode)
{
    if(mode == 'over')
    {
        sender.className = 'styleButtonOver';
    }
    else
    {   
        sender.className = 'styleButton';
    }
}

function showRegister(mode, lblError)
{
   var lblError = $get(lblError);
   if(lblError != null)
   {
    lblError.style.display="none";
   }
    if(mode)
    {
        $get('divEmailTextbox').style.display = '';
        $get('divLoginButtons').style.display = 'none';
        $get('divRegisterButtons').style.display = '';
        
           
        
    }
    else
    {
        $get('divEmailTextbox').style.display = 'none';
        $get('divLoginButtons').style.display = '';
        $get('divRegisterButtons').style.display = 'none';
    }
}

function showObject(obj, mode, message)
{
    if(message)
    {
        if(typeof obj.value != 'undefined')
        {
            obj.value = message;
        }
        else if(typeof obj.innerHTML != 'undefined') 
        {
            obj.innerHTML = message;
        }
    }
    obj.style.display = mode;
}

function hideObject(obj)
{
    obj.style.display = 'none';
}

function checkEmail(email){
    var objRegExp  =/(^[a-z]([a-z_\.]*)@([a-z_\.]*)([.][a-z]{3})$)|(^[a-z]([a-z_\.]*)@([a-z_\.]*)(\.[a-z]{3})(\.[a-z]{2})*$)/i;
    return objRegExp.test(email);
}

//function onRegisterUser(userNameID, passwordID, emaiID, validateID)
//{
//    var userName = $get(userNameID);
//    var password = $get(passwordID);
//    var email = $get(emaiID);
//    var validate = $get(validateID);
//  
//    
//    
//    if(userName && password && email && validate)
//    {
//        showObject(validate, 'block', 'Registering...');
//        if(userName.value.length == 0)
//        {
//            showObject(validate, 'block', "UserName can't left blank");
//            return;
//        }
//        if(password.value.length == 0)
//        {
//            showObject(validate, 'block', "Password can't left blank");
//            return;
//        }
//        if(email.value.length == 0)
//        {
//            showObject(validate, 'block', "Email can't left blank");
//            return;
//        }
//        if(!checkEmail(email.value))
//        {
//            showObject(validate, 'block', "Email isn't correct");
//            return;
//        }
//        PageMethods.RegisterUser(userName.value, password.value, email.value,
//            function(result){
//                if(result)
//                {
//                    showObject(validate, 'block', 'User Created Successfully');
//                    userName.value = '';
//                    password.value = '';
//                    email.value = '';
//                    showRegister(false);
//                } 
//                else
//                {
//                    showObject(validate, 'block', 'Registration Failed...');
//                }
//            },
//            function(exception)
//            {
//                showObject(validate, 'block', 'Registration Failed...');
//                alert(exception.get_message());
//            }
//        );  
//    }
//}

//function onLoginUser(userNameID, passwordID, validateID)
//{
//    var userName = $get(userNameID);
//    var password = $get(passwordID);
//    var validate = $get(validateID);
//    if(userName && password && validate)
//    {
//        showObject(validate, 'block', 'Checking...');
//        if(userName.value.length == 0)
//        {
//            showObject(validate, 'block', "UserName can't left blank");
//            return;
//        }
//        if(password.value.length == 0)
//        {
//            showObject(validate, 'block', "Password can't left blank");
//            return;
//        }
//        PageMethods.LoginUser(userName.value, password.value,
//            function(result){
//                if(result.Key)
//                {
//                    $get('spanTopUserName').innerHTML = ' ( ' + result.Value + ' )';
//                    $get('divLoginPanel').style.display = 'none';
//                    hideObject(validate);
//                } 
//                else
//                {
//                    showObject(validate, 'block', result.Value);
//                }
//                userName.value = '';
//                password.value = '';
//            },
//            function(exception)
//            {
//                showObject(validate, 'block', 'Login Failed...');
//                alert(exception.get_message());
//            }
//        );  
//    }
//}
function onAddBuddy(tbAddBuddyID, pnlBuddiesID)
{
    var tbAddBuddy = $get(tbAddBuddyID);
    var pnlBuddies = $get(pnlBuddiesID);
    
    
        alert("You must login, to add buddy.");
        tbAddBuddy.value='';
        return;
   
}
//function onDrop( sender, e )
//{
//    var container = e.get_container();
//    var item = e.get_droppedItem();
//    var position = e.get_position();
//    
//    //alert( String.format( "Container: {0}, Item: {1}, Position: {2}", container.id, item.id, position ) );
//    
//    var instanceId = parseInt(item.getAttribute("InstanceId"));
//    var columnNo = parseInt(container.getAttribute("columnNo"));
//    var row = position;
//    //WidgetService.MoveWidgetInstance( instanceId, columnNo, row );
//}
//function onNewItemDrop(sender,e){
//    var container = e.get_container();
//    var item = e.get_droppedItem();
//    var position = e.get_position();
//    var type = e.get_type();
//    
//    if(type == 'Chat')
//    {
//        var panel = sender.get_element();
//        var chatControlContent=document.createElement("div");
//        chatControlContent.id=container.id+position;
//        panel.appendChild(chatControlContent);    
//        $create(Chat.Presentation.ChatControlBehavior,{
//                'title':"WidgetBase",
//                'widgetHeaderCss':"widget_header",
//                'widgetTopLeftCss':"rpTopLeft",
//                'widgetTopRightCss':"rpTopRight",
//                'widgetTopRowCss':"rpTopRow",
//                'widgetButtonMinCss':"widget_buttonmin",
//                'widgetButtonMaxCss':"widget_buttonMax",
//                'widgetButtonRightMinCss':"widget_buttonRight_min",
//                'widgetButtonCloseCss':"widget_buttonClose",
//                'widgetBotLeftCss':"rpBotLeft",
//                'widgetBotRightCss':"rpBotRight",
//                'widgetBotCss':"rpBot",
//                'widgetMidLeftCss':"rpMidLeft",
//                'widgetMidRightCss':"rpMidRight",
//                'widgetMidContentCss':"rpMidContent",
//                'chatStyle' : "chatBackground",
//                'chatLabelStyle' : "chatLabelDisplay",
//                'chatTextAreaStyle' : "chatTextArea",
//                'btnViewHistoryStyle' : "chatBtnView",
//                'checkBoxStyle' : "chatCheckBox"}, {'keydown' : onChatTextKeyDown},null,chatControlContent);
//        if(typeof sender.appendFloatingBeh == 'function')
//        {
//            sender.appendFloatingBeh(chatControlContent);
//        }
//    }
//   
//    else if (type == 'Co-Authering')
//    {
//        var panel = sender.get_element();
//        var coAuthControlContent = document.createElement("div");
//        coAuthControlContent.id = container.id + position;
//        panel.appendChild(coAuthControlContent);  
//        $create(CoAuthering.Presentation.CoAuthBehavior,{
//                'title':"CoAuthering",
//                'widgetHeaderCss':"widget_header",
//                'widgetTopLeftCss':"rpTopLeft",
//                'widgetTopRightCss':"rpTopRight",
//                'widgetTopRowCss':"rpTopRow",
//                'widgetButtonMinCss':"widget_buttonmin",
//                'widgetButtonMaxCss':"widget_buttonMax",
//                'widgetButtonRightMinCss':"widget_buttonRight_min",
//                'widgetButtonCloseCss':"widget_buttonClose",
//                'widgetBotLeftCss':"rpBotLeft",
//                'widgetBotRightCss':"rpBotRight",
//                'widgetBotCss':"rpBot",
//                'widgetMidLeftCss':"rpMidLeft",
//                'widgetMidRightCss':"rpMidRight",
//                'widgetMidContentCss':"rpMidContent",
//                'generalStyle' : "coAuthBackground",
//                'buttonStyle' : "coAuthBtn",
//                'textStyle' : "chatTextArea",
//                'imagesLocation' : "Images/CoAuth/"},
//                null, null, coAuthControlContent);
//        if(typeof sender.appendFloatingBeh == 'function')
//        {
//            sender.appendFloatingBeh(coAuthControlContent);
//        }
//    }
// 
//    
////    $create(Chat.Presentation.ChatControlBehavior, {
////                    'title' : "Chat",
////                    'chatStyle' : "chatBackground",
////                    'chatLabelStyle' : "chatLabelDisplay",
////                    'chatTextAreaStyle' : "chatTextArea",
////                    'btnViewHistoryStyle' : "chatBtnView",
////                    'checkBoxStyle' : "chatCheckBox"},null, null,
////                    panel);
//    //__doPostBack("AddNewWidget", sender.get_DropCueID().substring(7,sender.get_DropCueID().length));
//}

//function onChatTextKeyDown(sender, ev)
//{
//    //alert('f');
//  // ChatWebService.HelloWorld(SucceededCallback);
//    ChatWebService.Send(sender.get_message(),
//        function(result)
//        {
//            if(result)
//            {
//                sender.set_displayName(result);
//                sender._taChat.value += sender.get_displayName() + " Says : " + sender.get_message() + "\r\n";
//            }
//        }
//    );
//}

function WidgetMinimize_Click(sender,midLeftID)
{
    var midLeft = $get(midLeftID);
    if(midLeft)
    {
    if(midLeft.style.display == "none")
    {
        midLeft.style.display='';
        sender.className = 'widget_buttonmin';
        }
    else
    {
        midLeft.style.display = "none";
        sender.className = 'widget_buttonMax';
        }
    }
}

function showButton(btnMinID,btnClouseId,titleVisibleID, bShow){
    var min =$get(btnMinID);
    var close =$get(btnClouseId);
    var title =$get(titleVisibleID);
    if(close){
        if(close.style.display=="none"){
            if(bShow){
                close.style.display="block";
            }
        }
        else if(!bShow){
             close.style.display="none";
        }
    }
    if(min){
        if(min.style.display=="none"){
            if(bShow){
                min.style.display="block";
            }
        }
        else if(!bShow){
             min.style.display="none";
        }
    }
    if(title){
        if(title.style.display=="none"){
            if(bShow){
                title.style.display="block";
            }
        }
        else if(!bShow){
             title.style.display="none";
        }
    }
   
}
function CallWebService()
{
   //WebService_1.HelloWorld(SucceededCallback);
}
function SucceededCallback(result, eventArgs)
{
    alert(result);
}
//function Chat_TextBox_Onkeydown()
//{

//}

