<%@page contentType="text/html;charset=UTF-8"%>
<% request.setCharacterEncoding("UTF-8"); %>
<HTML>
<HEAD>
<TITLE>Result</TITLE>
</HEAD>
<BODY>
<H1>Result</H1>

<jsp:useBean id="sampleServiceProxyid" scope="session" class="P2PWebService.ServiceProxy" />
<%
if (request.getParameter("endpoint") != null && request.getParameter("endpoint").length() > 0)
sampleServiceProxyid.setEndpoint(request.getParameter("endpoint"));
%>

<%
String method = request.getParameter("method");
int methodID = 0;
if (method == null) methodID = -1;

if(methodID != -1) methodID = Integer.parseInt(method);
boolean gotMethod = false;

try {
switch (methodID){ 
case 2:
        gotMethod = true;
        java.lang.String getEndpoint2mtemp = sampleServiceProxyid.getEndpoint();
if(getEndpoint2mtemp == null){
%>
<%=getEndpoint2mtemp %>
<%
}else{
        String tempResultreturnp3 = org.eclipse.jst.ws.util.JspUtils.markup(String.valueOf(getEndpoint2mtemp));
        %>
        <%= tempResultreturnp3 %>
        <%
}
break;
case 5:
        gotMethod = true;
        String endpoint_0id=  request.getParameter("endpoint8");
            java.lang.String endpoint_0idTemp = null;
        if(!endpoint_0id.equals("")){
         endpoint_0idTemp  = endpoint_0id;
        }
        sampleServiceProxyid.setEndpoint(endpoint_0idTemp);
break;
case 10:
        gotMethod = true;
        P2PWebService.Service getService10mtemp = sampleServiceProxyid.getService();
if(getService10mtemp == null){
%>
<%=getService10mtemp %>
<%
}else{
        if(getService10mtemp!= null){
        String tempreturnp11 = getService10mtemp.toString();
        %>
        <%=tempreturnp11%>
        <%
        }}
break;
case 13:
        gotMethod = true;
        String username_1id=  request.getParameter("username16");
            java.lang.String username_1idTemp = null;
        if(!username_1id.equals("")){
         username_1idTemp  = username_1id;
        }
        String filename_2id=  request.getParameter("filename18");
            java.lang.String filename_2idTemp = null;
        if(!filename_2id.equals("")){
         filename_2idTemp  = filename_2id;
        }
        boolean isShared13mtemp = sampleServiceProxyid.isShared(username_1idTemp,filename_2idTemp);
        String tempResultreturnp14 = org.eclipse.jst.ws.util.JspUtils.markup(String.valueOf(isShared13mtemp));
        %>
        <%= tempResultreturnp14 %>
        <%
break;
case 20:
        gotMethod = true;
        String username_3id=  request.getParameter("username23");
            java.lang.String username_3idTemp = null;
        if(!username_3id.equals("")){
         username_3idTemp  = username_3id;
        }
        String password_4id=  request.getParameter("password25");
            java.lang.String password_4idTemp = null;
        if(!password_4id.equals("")){
         password_4idTemp  = password_4id;
        }
        boolean authorize20mtemp = sampleServiceProxyid.authorize(username_3idTemp,password_4idTemp);
        String tempResultreturnp21 = org.eclipse.jst.ws.util.JspUtils.markup(String.valueOf(authorize20mtemp));
        %>
        <%= tempResultreturnp21 %>
        <%
break;
case 27:
        gotMethod = true;
        String username_5id=  request.getParameter("username30");
            java.lang.String username_5idTemp = null;
        if(!username_5id.equals("")){
         username_5idTemp  = username_5id;
        }
        String address_6id=  request.getParameter("address32");
            java.lang.String address_6idTemp = null;
        if(!address_6id.equals("")){
         address_6idTemp  = address_6id;
        }
        String port_7id=  request.getParameter("port34");
        int port_7idTemp  = Integer.parseInt(port_7id);
        sampleServiceProxyid.updateAddress(username_5idTemp,address_6idTemp,port_7idTemp);
break;
case 36:
        gotMethod = true;
        String username_8id=  request.getParameter("username39");
            java.lang.String username_8idTemp = null;
        if(!username_8id.equals("")){
         username_8idTemp  = username_8id;
        }
        java.lang.String[] getSharedFiles36mtemp = sampleServiceProxyid.getSharedFiles(username_8idTemp);
if(getSharedFiles36mtemp == null){
%>
<%=getSharedFiles36mtemp %>
<%
}else{
        String tempreturnp37 = null;
        if(getSharedFiles36mtemp != null){
        java.util.List listreturnp37= java.util.Arrays.asList(getSharedFiles36mtemp);
        tempreturnp37 = listreturnp37.toString();
        }
        %>
        <%=tempreturnp37%>
        <%
}
break;
case 41:
        gotMethod = true;
        String username_9id=  request.getParameter("username44");
            java.lang.String username_9idTemp = null;
        if(!username_9id.equals("")){
         username_9idTemp  = username_9id;
        }
        String filename_10id=  request.getParameter("filename46");
            java.lang.String filename_10idTemp = null;
        if(!filename_10id.equals("")){
         filename_10idTemp  = filename_10id;
        }
        sampleServiceProxyid.stopShare(username_9idTemp,filename_10idTemp);
break;
case 48:
        gotMethod = true;
        java.lang.String sayHello48mtemp = sampleServiceProxyid.sayHello();
if(sayHello48mtemp == null){
%>
<%=sayHello48mtemp %>
<%
}else{
        String tempResultreturnp49 = org.eclipse.jst.ws.util.JspUtils.markup(String.valueOf(sayHello48mtemp));
        %>
        <%= tempResultreturnp49 %>
        <%
}
break;
case 51:
        gotMethod = true;
        String username_11id=  request.getParameter("username54");
            java.lang.String username_11idTemp = null;
        if(!username_11id.equals("")){
         username_11idTemp  = username_11id;
        }
        String filename_12id=  request.getParameter("filename56");
            java.lang.String filename_12idTemp = null;
        if(!filename_12id.equals("")){
         filename_12idTemp  = filename_12id;
        }
        sampleServiceProxyid.share(username_11idTemp,filename_12idTemp);
break;
case 58:
        gotMethod = true;
        String username_13id=  request.getParameter("username61");
            java.lang.String username_13idTemp = null;
        if(!username_13id.equals("")){
         username_13idTemp  = username_13id;
        }
        String filename_14id=  request.getParameter("filename63");
            java.lang.String filename_14idTemp = null;
        if(!filename_14id.equals("")){
         filename_14idTemp  = filename_14id;
        }
        java.lang.String[] findFile58mtemp = sampleServiceProxyid.findFile(username_13idTemp,filename_14idTemp);
if(findFile58mtemp == null){
%>
<%=findFile58mtemp %>
<%
}else{
        String tempreturnp59 = null;
        if(findFile58mtemp != null){
        java.util.List listreturnp59= java.util.Arrays.asList(findFile58mtemp);
        tempreturnp59 = listreturnp59.toString();
        }
        %>
        <%=tempreturnp59%>
        <%
}
break;
}
} catch (Exception e) { 
%>
Exception: <%= org.eclipse.jst.ws.util.JspUtils.markup(e.toString()) %>
Message: <%= org.eclipse.jst.ws.util.JspUtils.markup(e.getMessage()) %>
<%
return;
}
if(!gotMethod){
%>
result: N/A
<%
}
%>
</BODY>
</HTML>