# Auto-generated by EclipseNSIS Script Wizard
# Feb 4, 2014 8:56:53 PM
SetCompressor /SOLID /FINAL lzma

Name "amp#"

# General Symbol Definitions
!define REGKEY "SOFTWARE\$(^Name)"
!define VERSION 1.1.1.0
!define COMPANY VPKSoft
!define URL http://www.vpksoft.net

# MUI Symbol Definitions
!define MUI_ICON ..\icon.ico
!define MUI_FINISHPAGE_NOAUTOCLOSE
!define MUI_STARTMENUPAGE_REGISTRY_ROOT HKLM
!define MUI_STARTMENUPAGE_NODISABLE
!define MUI_STARTMENUPAGE_REGISTRY_KEY ${REGKEY}
!define MUI_STARTMENUPAGE_REGISTRY_VALUENAME StartMenuGroup
!define MUI_STARTMENUPAGE_DEFAULTFOLDER "amp#"
!define MUI_UNICON .\un_icon.ico
!define MUI_UNFINISHPAGE_NOAUTOCLOSE
!define MUI_LANGDLL_REGISTRY_ROOT HKLM
!define MUI_LANGDLL_REGISTRY_KEY ${REGKEY}
!define MUI_LANGDLL_REGISTRY_VALUENAME InstallerLanguage
!define MUI_FINISHPAGE_RUN # this needs to be not-defined for the MUI_FINISHPAGE_RUN_FUNCTION to work..
!define MUI_FINISHPAGE_RUN_FUNCTION "RunAsCurrentUser" # The check box for a query whether to run the installed software as the current user after the installation..

BrandingText "amp#"

#Include the LogicLib
!include 'LogicLib.nsh'
!include "x64.nsh"
!include "FileAssoc.nsh"
!include "InstallOptions.nsh"
!include "DotNetChecker.nsh"
!include "nsProcess.nsh"

# Included files
!include Sections.nsh
!include MUI2.nsh

# Reserved Files
!insertmacro MUI_RESERVEFILE_LANGDLL

# Variables
Var StartMenuGroup
Var /GLOBAL ASSOC_MP3
Var /GLOBAL ASSOC_OGG
Var /GLOBAL ASSOC_FLAC
Var /GLOBAL ASSOC_WMA
Var /GLOBAL ASSOC_WAV
Var /GLOBAL ASSOC_AAC
Var /GLOBAL ASSOC_AIF

# Installer pages
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE ..\LICENSE
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_STARTMENU Application $StartMenuGroup
Page Custom PageAssociation
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

# Installer languages
!insertmacro MUI_LANGUAGE English
!insertmacro MUI_LANGUAGE Finnish

# Installer attributes
OutFile setup_ampsharp_1_1_1_0.exe
InstallDir "$PROGRAMFILES64\amp#"
CRCCheck on
XPStyle on
ShowInstDetails hide
VIProductVersion 1.1.1.0
VIAddVersionKey /LANG=${LANG_ENGLISH} ProductName "amp# installer"
VIAddVersionKey /LANG=${LANG_ENGLISH} ProductVersion "${VERSION}"
VIAddVersionKey /LANG=${LANG_ENGLISH} CompanyName "${COMPANY}"
VIAddVersionKey /LANG=${LANG_ENGLISH} CompanyWebsite "${URL}"
VIAddVersionKey /LANG=${LANG_ENGLISH} FileVersion "${VERSION}"
VIAddVersionKey /LANG=${LANG_ENGLISH} FileDescription "amp#"
VIAddVersionKey /LANG=${LANG_ENGLISH} LegalCopyright "Copyright © VPKSoft 2018"
InstallDirRegKey HKLM "${REGKEY}" Path
ShowUninstDetails hide

# Installer sections
Section -Main SEC0000
    SetOutPath $INSTDIR
    SetOverwrite on
			
	${nsProcess::FindProcess} "amp.exe" $R0
    ${If} $R0 == 0
		${nsProcess::CloseProcess} "amp.exe" $R0
	${EndIf}
	
	${nsProcess::Unload}
	
	File /r ..\amp\bin\Release\*.*
	
	File ..\LICENSE
		
	File .\languages.ico    
	
	SetOutPath $INSTDIR\licenses
	File ..\licenses\NAudio.license.txt
	File ..\licenses\NAudioFLAC.LICENSE
	File ..\licenses\NVorbis.COPYING
	File ..\licenses\taglib.lgpl-2.1.txt
	File ..\licenses\VPKSoft.Utils.COPYING
	File ..\licenses\VPKSoft.Utils.COPYING.LESSER
	    
    SetOutPath "$LOCALAPPDATA\amp#"
    File ..\amp\Localization\lang.sqlite
	
	!insertmacro CheckNetFramework 47
	  
    Call Associate
    
    SetOutPath $SMPROGRAMS\$StartMenuGroup
    CreateShortcut "$SMPROGRAMS\$StartMenuGroup\amp#.lnk" $INSTDIR\amp.exe
	CreateShortcut "$SMPROGRAMS\$StartMenuGroup\$(LocalizeDesc).lnk" "$INSTDIR\amp.exe" '"--localize=$LOCALAPPDATA\amp#\lang.sqlite" ' "$INSTDIR\languages.ico" 0
	
    WriteRegStr HKLM "${REGKEY}\Components" Main 1
SectionEnd

Section -post SEC0001
    WriteRegStr HKLM "${REGKEY}" Path $INSTDIR
    SetOutPath $INSTDIR
    WriteUninstaller $INSTDIR\uninstall_ampsharp.exe
    !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
    SetOutPath $SMPROGRAMS\$StartMenuGroup
    CreateShortcut "$SMPROGRAMS\$StartMenuGroup\$(^UninstallLink).lnk" $INSTDIR\uninstall_ampsharp.exe
    !insertmacro MUI_STARTMENU_WRITE_END
    WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" DisplayName "$(^Name)"
    WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" DisplayVersion "${VERSION}"
    WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" Publisher "${COMPANY}"
    WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" URLInfoAbout "${URL}"
    WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" DisplayIcon $INSTDIR\uninstall_ampsharp.exe
    WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" UninstallString $INSTDIR\uninstall_ampsharp.exe
    WriteRegDWORD HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" NoModify 1
    WriteRegDWORD HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" NoRepair 1
SectionEnd

# Macro for selecting uninstaller sections
!macro SELECT_UNSECTION SECTION_NAME UNSECTION_ID
    Push $R0
    ReadRegStr $R0 HKLM "${REGKEY}\Components" "${SECTION_NAME}"
    StrCmp $R0 1 0 next${UNSECTION_ID}
    !insertmacro SelectSection "${UNSECTION_ID}"
    GoTo done${UNSECTION_ID}
next${UNSECTION_ID}:
    !insertmacro UnselectSection "${UNSECTION_ID}"
done${UNSECTION_ID}:
    Pop $R0
!macroend

# a function to execute the installed software as non-administrator.. 
Function RunAsCurrentUser	
	ShellExecAsUser::ShellExecAsUser "" "$INSTDIR\amp.exe"
FunctionEnd

# Uninstaller sections
Section /o -un.Main UNSEC0000
    Delete /REBOOTOK "$SMPROGRAMS\$StartMenuGroup\amp#.lnk"
	Delete /REBOOTOK "$SMPROGRAMS\$StartMenuGroup\$(LocalizeDesc).lnk"	
	
	${nsProcess::FindProcess} "amp.exe" $R0
    ${If} $R0 == 0
		${nsProcess::CloseProcess} "amp.exe" $R0
	${EndIf}	
	
	${nsProcess::Unload}
	
	RMDir /r /REBOOTOK $INSTDIR	
	    
	Call un.DeleteLocalData
	
    DeleteRegValue HKLM "${REGKEY}\Components" Main
SectionEnd

Section -un.post UNSEC0001
    DeleteRegKey HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)"
    Delete /REBOOTOK "$SMPROGRAMS\$StartMenuGroup\$(^UninstallLink).lnk"
    Delete /REBOOTOK $INSTDIR\uninstall_ampsharp.exe
    DeleteRegValue HKLM "${REGKEY}" StartMenuGroup
    DeleteRegValue HKLM "${REGKEY}" Path
    DeleteRegKey /IfEmpty HKLM "${REGKEY}\Components"
    DeleteRegKey /IfEmpty HKLM "${REGKEY}"
    !insertmacro APP_UNASSOCIATE "mp3" $(MUSAmp)
    !insertmacro APP_UNASSOCIATE "ogg" $(MUSOgg)
    !insertmacro APP_UNASSOCIATE "flac" $(MUSFlac)
    !insertmacro APP_UNASSOCIATE "wma" $(MUSWma)
    !insertmacro APP_UNASSOCIATE "wav" $(MUSAmp)
    !insertmacro APP_UNASSOCIATE "aif" $(MUSAif)
    !insertmacro APP_UNASSOCIATE "aiff" $(MUSAif)
    !insertmacro APP_UNASSOCIATE "aac" $(MUSAac)
    !insertmacro APP_UNASSOCIATE "m4a" $(MUSAac)	
    !insertmacro APP_UNASSOCIATE "amp#_qex" $(MUSQEXP)
	
    !insertmacro UPDATEFILEASSOC    
    RmDir /REBOOTOK $SMPROGRAMS\$StartMenuGroup
    RmDir /REBOOTOK "$LOCALAPPDATA\amp#"
    RmDir /REBOOTOK $INSTDIR
SectionEnd

# Installer functions
Function .onInit
    InitPluginsDir
    !insertmacro MUI_LANGDLL_DISPLAY    
FunctionEnd

# Uninstaller functions
Function un.onInit
    ReadRegStr $INSTDIR HKLM "${REGKEY}" Path
    !insertmacro MUI_STARTMENU_GETFOLDER Application $StartMenuGroup
    !insertmacro MUI_UNGETLANGUAGE
    !insertmacro SELECT_UNSECTION Main ${UNSEC0000}
FunctionEnd

Function un.DeleteLocalData
	MessageBox MB_ICONQUESTION|MB_YESNO "$(DeleteUserData)" IDYES deletelocal IDNO nodeletelocal
	deletelocal:
    RMDir /r /REBOOTOK "$LOCALAPPDATA\amp#"
	nodeletelocal:
FunctionEnd

# Installer Language Strings
# TODO Update the Language Strings with the appropriate translations.

LangString ^UninstallLink ${LANG_ENGLISH} "Uninstall $(^Name)"
LangString ^UninstallLink ${LANG_FINNISH} "Poista $(^Name)"

LangString AssocDesc ${LANG_FINNISH} "Avaa sovelluksella amp#"
LangString AssocDesc ${LANG_ENGLISH} "Open with amp#"

LangString AssocDesc2 ${LANG_FINNISH} "Valitse tiedostosidokset"
LangString AssocDesc2 ${LANG_ENGLISH} "Choose file associations"

LangString AssocDesc3 ${LANG_FINNISH} "Valitse tiedostosidokset ohjelman käyttöön"
LangString AssocDesc3 ${LANG_ENGLISH} "Choose file associations for to use with the program"

LangString MUSAmp  ${LANG_ENGLISH} "amp# mp3 file"
LangString MUSOgg  ${LANG_ENGLISH} "amp# ogg file"
LangString MUSFlac ${LANG_ENGLISH} "amp# flac file"
LangString MUSWma  ${LANG_ENGLISH} "amp# wma file"
LangString MUSWav  ${LANG_ENGLISH} "amp# wav file"
LangString MUSAac  ${LANG_ENGLISH} "amp# m4a/aac file"
LangString MUSAif  ${LANG_ENGLISH} "amp# aif/aiff file"
LangString MUSQEXP  ${LANG_ENGLISH} "amp# queue export file"

LangString MUSAmp2  ${LANG_ENGLISH} "mp3 file"
LangString MUSOgg2  ${LANG_ENGLISH} "ogg file"
LangString MUSFlac2 ${LANG_ENGLISH} "flac file"
LangString MUSWma2  ${LANG_ENGLISH} "wma file"
LangString MUSWav2  ${LANG_ENGLISH} "wav file"
LangString MUSAac2  ${LANG_ENGLISH} "m4a/aac file"
LangString MUSAif2  ${LANG_ENGLISH} "aif/aiff file"
LangString MUSQEXP2  ${LANG_ENGLISH} "queue export file"

LangString MUSAmp  ${LANG_FINNISH} "amp# mp3 -tiedosto"
LangString MUSOgg  ${LANG_FINNISH} "amp# ogg -tiedosto"
LangString MUSFlac ${LANG_FINNISH} "amp# flac -tiedosto"
LangString MUSWma  ${LANG_FINNISH} "amp# wma -tiedosto"
LangString MUSWav  ${LANG_FINNISH} "amp# wav -tiedosto"
LangString MUSAac  ${LANG_FINNISH} "amp# m4a/aac -tiedosto"
LangString MUSAif  ${LANG_FINNISH} "amp# aif/aiff -tiedosto"
LangString MUSQEXP  ${LANG_FINNISH} "amp# jono -tiedosto"

LangString MUSAmp2  ${LANG_FINNISH} "mp3 -tiedosto"
LangString MUSOgg2  ${LANG_FINNISH} "ogg -tiedosto"
LangString MUSFlac2 ${LANG_FINNISH} "flac -tiedosto"
LangString MUSWma2  ${LANG_FINNISH} "wma -tiedosto"
LangString MUSWav2  ${LANG_FINNISH} "wav -tiedosto"
LangString MUSAac2  ${LANG_FINNISH} "m4a/aac -tiedosto"
LangString MUSAif2  ${LANG_FINNISH} "aif/aiff -tiedosto"
LangString MUSQEXP2  ${LANG_FINNISH} "jono -tiedosto"

LangString LocalizeDesc ${LANG_FINNISH} "Lokalisointi"
LangString LocalizeDesc ${LANG_ENGLISH} "Localization"

LangString DeleteUserData ${LANG_FINNISH} "Poista paikalliset käyttäjätiedot?"
LangString DeleteUserData ${LANG_ENGLISH} "Delete local user data?"

Function PageAssociation
  # If you need to skip the page depending on a condition, call Abort.
  ReserveFile "amp_sharp_association_1033.ini"
  ReserveFile "amp_sharp_association_1035.ini"
  !insertmacro INSTALLOPTIONS_EXTRACT "amp_sharp_association_1033.ini"
  !insertmacro INSTALLOPTIONS_EXTRACT "amp_sharp_association_1035.ini"
  !insertmacro MUI_HEADER_TEXT $(AssocDesc2) $(AssocDesc3)  
  !insertmacro INSTALLOPTIONS_DISPLAY "amp_sharp_association_$Language.ini"
  !insertmacro INSTALLOPTIONS_READ $ASSOC_MP3  "amp_sharp_association_$Language.ini" "Field 1" "State"
  !insertmacro INSTALLOPTIONS_READ $ASSOC_OGG  "amp_sharp_association_$Language.ini" "Field 2" "State"
  !insertmacro INSTALLOPTIONS_READ $ASSOC_FLAC "amp_sharp_association_$Language.ini" "Field 3" "State"
  !insertmacro INSTALLOPTIONS_READ $ASSOC_WMA  "amp_sharp_association_$Language.ini" "Field 4" "State"
  !insertmacro INSTALLOPTIONS_READ $ASSOC_WAV  "amp_sharp_association_$Language.ini" "Field 5" "State"
  !insertmacro INSTALLOPTIONS_READ $ASSOC_AAC  "amp_sharp_association_$Language.ini" "Field 6" "State"
  !insertmacro INSTALLOPTIONS_READ $ASSOC_AIF  "amp_sharp_association_$Language.ini" "Field 7" "State"
FunctionEnd

Function Associate
  !insertmacro APP_UNASSOCIATE "mp3" $(MUSAmp)
  !insertmacro APP_UNASSOCIATE "ogg" $(MUSOgg)
  !insertmacro APP_UNASSOCIATE "flac" $(MUSFlac)
  !insertmacro APP_UNASSOCIATE "wma" $(MUSWma)
  !insertmacro APP_UNASSOCIATE "wav" $(MUSAmp)
  !insertmacro APP_UNASSOCIATE "aif" $(MUSAif)
  !insertmacro APP_UNASSOCIATE "aiff" $(MUSAif)
  !insertmacro APP_UNASSOCIATE "aac" $(MUSAac)
  !insertmacro APP_UNASSOCIATE "m4a" $(MUSAac)
  !insertmacro APP_UNASSOCIATE "amp#_qex" $(MUSQEXP)

  ${If} $ASSOC_MP3 == "1"
    Call AssociateMP3
  ${EndIf}
  
  ${If} $ASSOC_OGG == "1"
    Call AssociateOGG
  ${EndIf}
  
  ${If} $ASSOC_FLAC == "1"
    Call AssociateFLAC
  ${EndIf}
  
  ${If} $ASSOC_WMA == "1"
    Call AssociateWMA
  ${EndIf}
   
  ${If} $ASSOC_WAV == "1"
    Call AssociateWAV
  ${EndIf}
  
  ${If} $ASSOC_AAC == "1"
    Call AssociateAAC
  ${EndIf}  

  ${If} $ASSOC_AIF == "1"
    Call AssociateAIF
  ${EndIf}    
  
  Call AssociateQEXP
  !insertmacro UPDATEFILEASSOC
FunctionEnd

Function AssociateMP3
  !insertmacro APP_ASSOCIATE "mp3" $(MUSAmp) $(MUSAmp2) \
      "$INSTDIR\amp.exe,0" "$(AssocDesc)" "$INSTDIR\amp.exe $\"%1$\""
FunctionEnd

Function AssociateOGG
  !insertmacro APP_ASSOCIATE "ogg" $(MUSOgg) $(MUSOgg2) \
      "$INSTDIR\amp.exe,0" $\"$(AssocDesc)$\" "$INSTDIR\amp.exe $\"%1$\""
FunctionEnd

Function AssociateFLAC
  !insertmacro APP_ASSOCIATE "flac" $(MUSFlac) $(MUSFlac2) \
      "$INSTDIR\amp.exe,0" $(AssocDesc) "$INSTDIR\amp.exe $\"%1$\""
FunctionEnd

Function AssociateWMA
  !insertmacro APP_ASSOCIATE "wma" $(MUSWma) $(MUSWma2) \
      "$INSTDIR\amp.exe,0" $(AssocDesc) "$INSTDIR\amp.exe $\"%1$\""
FunctionEnd

Function AssociateWAV
  !insertmacro APP_ASSOCIATE "wav" $(MUSWav) $(MUSWav2) \
      "$INSTDIR\amp.exe,0" $(AssocDesc) "$INSTDIR\amp.exe $\"%1$\""
FunctionEnd

Function AssociateQEXP
  !insertmacro APP_ASSOCIATE "amp#_qex" $(MUSQEXP) $(MUSQEXP2) \
      "$INSTDIR\amp.exe,0" $(AssocDesc) "$INSTDIR\amp.exe $\"%1$\""
FunctionEnd

Function AssociateAAC
  !insertmacro APP_ASSOCIATE "aac" $(MUSAac) $(MUSAac2) \
      "$INSTDIR\amp.exe,0" $(AssocDesc) "$INSTDIR\amp.exe $\"%1$\""
	  
  !insertmacro APP_ASSOCIATE "m4a" $(MUSAac) $(MUSAac2) \
      "$INSTDIR\amp.exe,0" $(AssocDesc) "$INSTDIR\amp.exe $\"%1$\""	  
FunctionEnd

Function AssociateAIF
  !insertmacro APP_ASSOCIATE "aif" $(MUSAif) $(MUSAif2) \
      "$INSTDIR\amp.exe,0" $(AssocDesc) "$INSTDIR\amp.exe $\"%1$\""
	  
  !insertmacro APP_ASSOCIATE "aiff" $(MUSAif) $(MUSAif2) \
      "$INSTDIR\amp.exe,0" $(AssocDesc) "$INSTDIR\amp.exe $\"%1$\""	  
FunctionEnd
