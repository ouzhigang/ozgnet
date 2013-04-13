////////////////////////////////////////////////////////////
////                                                    ////
////                 TreeAlertControls                  ////
////    Namespace:  TreeControls.js.TreeAlertMsg.js     ////
////    CreateName: Tree                                ////
////    Version:    1.0 bate2                           ////
////    CreateDate: 2007-5-9                            ////
////                                                    ////
////////////////////////////////////////////////////////////


/*
Remark: 1 : document.documentElement.offsetWidth - document.body.clientWidth = scroll.width;
*/

var iTree_Alert_MoveMaxHeight=0;
var iTree_Alert_MoveMaxWidth=0;
var iTree_Alert_scrolltop = 0;
var iTree_Alert_scrollleft = 0;
var iTree_Alert_pageHeight = 0;
var iTree_Alert_scrollwidth=0;

///////pageloadshowMsg
window.onload = function ()
{    
    iTree_Alert_pageHeight=document.body.clientHeight;
    iTree_Alert_pageHeight = document.documentElement.clientHeight>iTree_Alert_pageHeight?document.documentElement.clientHeight: iTree_Alert_pageHeight;
    
    if(TreeAlertMsg$('TreeOverlayBackgroundDiv')['style']['display']=="block")
    {
        TreeAlertMsg$('TreeOverlayBackgroundDiv').style.top=0;
        TreeAlertMsg$('TreeOverlayBackgroundDiv').style.height = iTree_Alert_pageHeight;
        iTree_Alert_MoveMaxHeight = iTree_Alert_pageHeight - parseInt(TreeAlertMsg$('TreeAlertMsgDiv').offsetHeight);    
        
        var sClientWidth = document.documentElement.clientWidth;
        var sWidth  = document.body.clientWidth;
        
        if(sClientWidth < sWidth)
        {
            iTree_Alert_scrollwidth = parseInt(document.documentElement.scrollWidth) - parseInt(document.body.clientWidth);
            iTree_Alert_MoveMaxWidth = parseInt(sWidth) - iTree_Alert_scrollwidth - parseInt(TreeAlertMsg$('TreeAlertMsgDiv').offsetWidth);
        }
        else if(sClientWidth == sWidth)
        {
            iTree_Alert_MoveMaxWidth = parseInt(document.documentElement.offsetWidth) - iTree_Alert_scrollwidth - parseInt(TreeAlertMsg$('TreeAlertMsgDiv').offsetWidth);
        }         
    }
}
/*ShowPage*/
function TreeAlertMsgShowMsg(_sMsg)                  
{
    TreeAlertMsg$('TreeOverlayBackgroundDiv').style.height = iTree_Alert_pageHeight;
    
    var sClientWidth = document.documentElement.clientWidth;
    var sWidth  = document.body.clientWidth;
   
    if(sWidth < sClientWidth)
    {
        TreeAlertMsg$('TreeOverlayBackgroundDiv').style.width = sClientWidth;
        iTree_Alert_MoveMaxWidth = sClientWidth - 398;
    }
    else if(sWidth == sClientWidth)
    {
        TreeAlertMsg$('TreeOverlayBackgroundDiv').style.width = sClientWidth;
        iTree_Alert_scrollwidth = parseInt(document.documentElement.offsetWidth) - parseInt(document.body.clientWidth);
        iTree_Alert_MoveMaxWidth = parseInt(document.documentElement.offsetWidth) - iTree_Alert_scrollwidth - 398;
    }
    else
    {
        TreeAlertMsg$('TreeOverlayBackgroundDiv').style.width = sWidth;
        iTree_Alert_MoveMaxWidth = parseInt(sWidth)- 398;        
    }
    
    TreeAlertMsg$('TreeAlertMsgTable_Table_MsgTd').innerText = _sMsg;
 
    var selectControls = document.getElementsByTagName("select");
    for(var i=0;i<selectControls.length;i++)
    {
        selectControls[i]['style']['visibility']="hidden";
    }

    TreeAlertMsg$('TreeOverlayBackgroundDiv')['style']['display']="block";
    TreeAlertMsg$('TreeAlertMsgDiv')['style']['display']="block";
    TreeAlertMsgmiddle("TreeAlertMsgDiv");
    
    iTree_Alert_scrolltop = parseInt(document.body.scrollTop);
    iTree_Alert_scrollleft = parseInt(document.body.scrollLeft); 
 }

/*HiddenMsg*/
function TreeAlertMsgHiddenMsg()
{
    TreeAlertMsg$('TreeOverlayBackgroundDiv')['style']['display']="none";
    TreeAlertMsg$('TreeAlertMsgDiv')['style']['display']="none";
    
    window.onscroll = null;
    
    var selectControls = document.getElementsByTagName("select");
    for(var i=0;i<selectControls.length;i++)
    {
        selectControls[i]['style']['visibility']="visible";
    }
}

/*window.onscroll*/
function TreeAlertMsgOnscroll()
{
    var   t   =   document.body.scrollTop;   
    var   l   =   document.body.scrollLeft;
    var oAlertMsgDiv = TreeAlertMsg$('TreeAlertMsgDiv');
    
    oAlertMsgDiv.style.top = parseInt(oAlertMsgDiv.style.top) + (t - iTree_Alert_scrolltop);
    oAlertMsgDiv.style.left = parseInt(oAlertMsgDiv.style.top) + (l - iTree_Alert_scrollleft);    
}

/*Msgmiddle*/
function TreeAlertMsgmiddle(_sId)
{
		var sClientWidth = document.documentElement.clientWidth;
		var sClientHeight = document.documentElement.clientHeight;
				
		var sScrollTop = document.documentElement.scrollTop;
		
		TreeAlertMsg$(_sId).style.position = "absolute";
		TreeAlertMsg$(_sId).style.left = ((document.documentElement.clientWidth / 2) - 398/2)+'px';		
		var sTop = (sClientHeight / 2) - 206 / 2;
		
		TreeAlertMsg$(_sId)['style']['top'] = sTop > 0 ? sTop+'px' : ((sClientHeight / 2 + sScrollTop) - 206 / 2)+'px';
}

/*GetObject*/
function TreeAlertMsg$(_sId)
{
    var oControls = document.getElementById(_sId);
    return oControls
}

/*windowSizeChange*/
window.onresize=function TreeAlertMsgSize()
{
    if(TreeAlertMsg$('TreeOverlayBackgroundDiv')['style']['display']=="block")
    {
        var oTreeAlertMsg = TreeAlertMsg$("TreeAlertMsgDiv");
        
        TreeAlertMsg$('TreeOverlayBackgroundDiv').style.height = iTree_Alert_pageHeight;
        iTree_Alert_MoveMaxHeight = iTree_Alert_pageHeight - parseInt(oTreeAlertMsg.offsetHeight);
                
        var sClientWidth = document.documentElement.clientWidth;
        var sWidth  = document.body.clientWidth;
        
        if(sWidth < sClientWidth)
        {
            TreeAlertMsg$('TreeOverlayBackgroundDiv').style.width = sClientWidth;
            iTree_Alert_MoveMaxWidth = sClientWidth - parseInt(oTreeAlertMsg.offsetWidth);
        }
        else
        {
            TreeAlertMsg$('TreeOverlayBackgroundDiv').style.width = sWidth-iTree_Alert_scrollwidth;
            iTree_Alert_MoveMaxWidth = sWidth-iTree_Alert_scrollwidth - parseInt(oTreeAlertMsg.offsetWidth);
        }
        TreeAlertMsgmiddle('TreeAlertMsgDiv');        
    }
}


var iTree_Alert_excursionX = 0;
var iTree_Alert_excursionY = 0;
/*MoveTreeAlertMsg*/
function TreeAlertMsgmoveStart(event)
{
        var oTreeAlertMsg = TreeAlertMsg$('TreeAlertMsgDiv');
        
        var oTreeAlertMsgTop = TreeAlertMsg$('TreeAlertMsgDiv').style.top;
        var oTreeAlertMsgLeft = TreeAlertMsg$('TreeAlertMsgDiv').style.left;
        
        var tempx = event.clientX + document.body.scrollLeft;
        var tempy = event.clientY + document.body.scrollTop;
        
        iTree_Alert_excursionX = parseInt(tempx) - parseInt(oTreeAlertMsgLeft);
        iTree_Alert_excursionY = parseInt(tempy) - parseInt(oTreeAlertMsgTop);
        
        oTreeAlertMsg.setCapture();
        oTreeAlertMsg.onmousemove = TreeAlertMsgmousemove;
        oTreeAlertMsg.onmouseup = TreeAlertMsgMouseup;
}

function TreeAlertMsgmousemove()
{
    var oEvent = window.event ? window.event : event;
    var tempmousex = event.clientX + document.body.scrollLeft;   
    var tempmousey = event.clientY + document.body.scrollTop;
   
    TreeAlertMsg$("TreeAlertMsgDiv").style.top = parseInt(tempmousey) - iTree_Alert_excursionY;
    TreeAlertMsg$("TreeAlertMsgDiv").style.left = parseInt(tempmousex) - iTree_Alert_excursionX;    
}

/*MoveTreeAlertMsg*/
function TreeAlertMsgMouseup()
{    
    var oTreeAlertMsg = TreeAlertMsg$("TreeAlertMsgDiv");
    var iAlertMsgTop = parseInt(oTreeAlertMsg.style.top);
    var iAlertMsgLeft = parseInt(oTreeAlertMsg.style.left);
    
    if(iAlertMsgTop < 0)
    {
        oTreeAlertMsg.style.top = 0;
    }
    else if(iAlertMsgTop > iTree_Alert_MoveMaxHeight-2)
    {
        oTreeAlertMsg.style.top = iTree_Alert_MoveMaxHeight-2;
    }
    
    if(iAlertMsgLeft < 0 )
    {
        oTreeAlertMsg.style.left = 0;
    }
    else if(iAlertMsgLeft > iTree_Alert_MoveMaxWidth)
    {
        oTreeAlertMsg.style.left = iTree_Alert_MoveMaxWidth;
    }
    
    oTreeAlertMsg.releaseCapture();
    oTreeAlertMsg.onmousemove = null;
    oTreeAlertMsg.onmouseup = null;
}