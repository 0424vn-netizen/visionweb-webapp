@echo off
attrib -R ..\AS.Web.UI\res\css\master.min.css
attrib -R ..\AS.Web.UI\App_Themes\Default\branding_master.min.css
attrib -R ..\AS.Web.UI\App_Themes\FIS\branding_master.min.css
lessc -yui-compress "..\AS.Web.UI\res\css\master.css" > "..\AS.Web.UI\res\css\master.min.css" & lessc -yui-compress "..\AS.Web.UI\App_Themes\Default\branding_master.css" > "..\AS.Web.UI\App_Themes\Default\branding_master.min.css" & lessc -yui-compress "..\AS.Web.UI\App_Themes\FIS\branding_master.css" > "..\AS.Web.UI\App_Themes\FIS\branding_master.min.css"