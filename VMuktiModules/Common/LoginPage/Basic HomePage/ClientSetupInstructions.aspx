<%@ page language="C#" autoeventwireup="true" title="VMukti_ClientSetupInstruction" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="Refresh" CONTENT="2; url=VMuktiClient1.0.zip"/>

    <style type="text/css">
        .NumberStyle
        {
            width: 50px;
            font-family: Calibri;
            font-size: 60px;
            font-weight: normal;
            color: #808080;
            text-align: right;
            vertical-align: top;
        }
        
        .caption
        {
            font-family: "times New Roman", Times, serif;
            font-size: large;
            font-weight: normal;
            color: #999999;
            text-align: justify;
            vertical-align: middle;
            width: 348px;
        }
        .style2
        {
            width: 292px;
        }
        .tableStyle
        {
            vertical-align: middle;
        }
        
        .mainpage
        {
            background-color: #FFFFFF;
            background-image: url('Images/Clientbg.jpg');
            background-repeat: no-repeat
        }
        
        .divstyle
        {
            font-family: "Times New Roman", Times, serif;
            font-size: large;
            color: #999999;
            text-align: center;
        }
        
        .thnksforchoosing
        {
            font-size: 50px;
            color: #FF6600;
            text-align: center;
            font-family: Calibri;
        }
        
        .style3
        {
            width: 290px;
        }
         .BlankColoum
        {
            width: 60px;
        }
        
    </style>
     
</head>
<body  class="mainpage" >
    <form id="form1" runat="server">
    <div class="thnksforchoosing" >Thanks for choosing VMukti !</div>
    <div class="divstyle">
    <b style="font-family: Calibri; font-size: xx-large; font-weight: Normal; color: white; text-align: center;" 
            >
    Just follow these easy steps to get started:</b><br />
        
    download should automatically begin in a few seconds,if not then, <a href="VMuktiClient1.0.zip">click here</a>. 
   </div>
   
   <table class="tableStyle" >
   
   <tr>
   
     <td />
         &nbsp;<td class="style3" >
           &nbsp;</td>
    
       <td class="BlankColoum" >
           &nbsp;</td>
    
     <td  />
         &nbsp;<td >
           &nbsp;</td>
    
    </tr>
    
   <tr>
   
     <td class="NumberStyle" />
         1.<td class="style3">
        <asp:Image ID="Image8" runat="server" BorderStyle="Solid" Height="221px" 
            ImageAlign="Left" ImageUrl="~/Images/Client1.png" style="margin-right: 0px" 
            Width="300px" />
        </td>
    
       <td class="BlankColoum">
           &nbsp;</td>
    
     <td class="NumberStyle" />
         2.<td class="style2">
        <asp:Image ID="Image9" runat="server" Height="221px" ImageUrl="~/Images/Client2.png" 
        style="margin-left: 0px" Width="300px" />
        </td>
    
    </tr>
    
   <tr>
   
     <td class="NumberStyle" />
         &nbsp;<td class="style3">
            <div class="caption" >Click open to start installation process.</div> 
        </td>
    
       <td class="BlankColoum">
           &nbsp;</td>
    
     <td class="NumberStyle" />
         &nbsp;<td class="style2">
            <div class="caption" >
                <p>
                    Give permission to launch the setup wizard by clicking Continue after the file 
                    has downloaded.</p>
            </div> 
        </td>
    
    </tr>
    
   <tr>
   
     <td class="NumberStyle" />
         <br />
         3.<td class="style3">
        <asp:Image ID="Image1" runat="server" BorderStyle="Solid" Height="221px" 
            ImageAlign="Left" ImageUrl="~/Images/Client3.png" style="margin-right: 0px" 
            Width="300px" />
        </td>
    
       <td class="BlankColoum">
           &nbsp;</td>
    
     <td class="NumberStyle" />
         <br />
         4.<td class="style2">
            <asp:Image ID="Image3" runat="server" Height="221px" 
                    ImageUrl="~/Images/Client4.png" Width="300px" />
        </td>
    
    </tr>
    
      <div></div>
      
    <tr>
     <td class="style1" />
        <td class="style3">
            <div CssClass="caption" class="caption"> 
                This installation would ask you to install.NET Framework 3.0/3.5, if your system 
                does not have .Net framework installed. 
            </div>
        </td>
        <td class="BlankColoum">
            &nbsp;</td>
        <td class="style1" />
        <td class="style2">
            <div class="caption" cssclass="caption">
                Click on yes button and Download the .NET framework just by browsing
                <a href="http://msdn2.microsoft.com/hi-in/netframework/aa569263.aspx">
                http://msdn2.microsoft.com/hi-in/netframework/aa569263.aspx</a>&nbsp;and clicking on 
                the link shown in red box.</div>
        </td>
    </tr>
  
    <tr>
     <td class="style1" />
         &nbsp;<td class="style3">
            &nbsp;</td>
        <td class="BlankColoum">
            &nbsp;</td>
        <td class="style1" />
            &nbsp;<td class="style2">
            &nbsp;</td>
    </tr>
  
    <tr>
        <td class="NumberStyle" />
            5.<td class="style3">
            <asp:Image ID="Image5" runat="server" Height="221px" 
                    ImageUrl="~/Images/Client5.png" Width="300px" />
                </td>
            
         <td class="BlankColoum">
             &nbsp;</td>
            
         <td class="NumberStyle" />
             6.<td class="style2">
        <asp:Image ID="Image10" runat="server" Height="221px" ImageUrl="~/Images/Client6.png" 
        style="margin-left: 0px" Width="300px" />
            </td>
        
    </tr>
    
    <tr>
        <td class="style1" />
    &nbsp;<td class="style3">
            <div class="caption" cssclass="caption">
               
                Now install the downloaded .Net Framework 3.5 setup by double clicking on 
                dotNetFx35setup.exe</div>
                </td>
            
         <td class="BlankColoum">
             &nbsp;</td>
            
         <td class="style1" />
             &nbsp;<td class="style2">
            <div class="caption" cssclass="caption">
               
                After Successful installation of .Net Framwork 3.5 VMukti setup wizard popup automatically 
                press Next to proceed VMukti installation.</div>
                </td>
        
    </tr>
    
    <tr>
        <td class="style1" />
            &nbsp;<td class="style3">
            &nbsp;</td>
            
         <td class="BlankColoum">
             &nbsp;</td>
            
         <td class="style1" />
             &nbsp;<td class="style2">
            &nbsp;</td>
        
    </tr>
    
    <tr>
        <td class="NumberStyle" />
                        7. <td class="style3">
                    <asp:Image ID="Image6" runat="server" Height="221px" 
                        ImageUrl="~/Images/Client7.png" Width="300px" />
            </td>
            
         <td class="BlankColoum">
                    &nbsp;</td>
            
         <td class="NumberStyle" />
             8.<td class="style2">
                    <asp:Image ID="Image7" runat="server" Height="221px" 
                        ImageUrl="~/Images/Client8.png" Width="300px" />
            </td>
        
    </tr>
    
    <tr>
        <td class="style1" />
            &nbsp;<td class="style3">
                    <div class="caption" cssclass="caption">
                       &nbsp;VMukti client installation completed. Click close to finish the installation</div>
            </td>
            
         <td class="BlankColoum">
                    &nbsp;</td>
            
         <td class="style1" />
             &nbsp;<td class="style2">
                    <div class="caption" cssclass="caption">
                        Browse <a href="http://betalive.vmukti.com">http://betalive.vmukti.com</a> and 
                        login/register to start collaboration.</tr>
    </table>
    
       
    &nbsp;</form>
</body>
</html>