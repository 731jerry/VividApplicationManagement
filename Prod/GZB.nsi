; 该脚本使用 HM VNISEdit 脚本编辑器向导产生

; 安装程序初始定义常量
!define PRODUCT_NAME "管账宝"
!define VERSION_NAME "精华版"
!define PRODUCT_NAME_ENG "GZB"
!define APP_NAME "管账宝"
!define PRODUCT_VERSION "1.6.5"
!define PRODUCT_PUBLISHER "唯达软件科技有限公司"
!define INSTALL_LOCATION "c:\GZB"
!define PRODUCT_DIR_REGKEY "Software\VividApp\${PRODUCT_NAME_ENG}\${APP_NAME}.exe"
!define PRODUCT_WEB_SITE "http://www.vividapp.net"
!define PRODUCT_BUILD_KEY "Software\VividApp\${PRODUCT_NAME_ENG}\"
!define PRODUCT_BUILD_ROOT_KEY "HKCU"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

SetCompressor lzma

; ------ MUI 现代界面定义 (1.67 版本以上兼容) ------
!include "MUI.nsh"

; MUI 预定义常量
!define MUI_ABORTWARNING
; MUI Settings / Icons
; 安装包图标
!define MUI_ICON "config\img\install.ico"
;!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"
;!define MUI_ICON "${NSISDIR}\Contrib\Graphics\Icons\orange-install.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\orange-uninstall.ico"

; MUI Settings / Header
!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_RIGHT
!define MUI_HEADERIMAGE_BITMAP "${NSISDIR}\Contrib\Graphics\Header\orange-r.bmp"
!define MUI_HEADERIMAGE_UNBITMAP "${NSISDIR}\Contrib\Graphics\Header\orange-uninstall-r.bmp"

; MUI Settings / Wizard
!define MUI_WELCOMEFINISHPAGE_BITMAP "${NSISDIR}\Contrib\Graphics\Wizard\orange.bmp"
!define MUI_UNWELCOMEFINISHPAGE_BITMAP "${NSISDIR}\Contrib\Graphics\Wizard\orange-uninstall.bmp"

; 欢迎页面
!insertmacro MUI_PAGE_WELCOME
; 许可协议页面
!insertmacro MUI_PAGE_LICENSE "config\lic.txt"
; 安装目录选择页面
;!insertmacro MUI_PAGE_DIRECTORY
; 安装过程页面
!insertmacro MUI_PAGE_INSTFILES
; 安装完成页面
!define MUI_FINISHPAGE_RUN "${APP_NAME}.exe"
!insertmacro MUI_PAGE_FINISH

; 安装卸载过程页面
!insertmacro MUI_UNPAGE_INSTFILES

; 安装界面包含的语言设置
!insertmacro MUI_LANGUAGE "SimpChinese"

; 安装预释放文件
!insertmacro MUI_RESERVEFILE_INSTALLOPTIONS
; ------ MUI 现代界面定义结束 ------

Name "${PRODUCT_NAME}-${VERSION_NAME}v${PRODUCT_VERSION}"
OutFile "${PRODUCT_NAME}-${VERSION_NAME}v${PRODUCT_VERSION}.exe"
InstallDir "${INSTALL_LOCATION}"
ShowInstDetails show
ShowUnInstDetails show

Section "MainSection" SEC01
  SetOutPath "${INSTALL_LOCATION}"
  ;SetOverwrite ifnewer
  ;SetOverwrite off
  SetOverwrite on ;覆盖安装
  File /r "${PRODUCT_NAME_ENG}\*.*"
SectionEnd

Section -AdditionalIcons
  SetOutPath $INSTDIR
  CreateDirectory "$SMPROGRAMS\${PRODUCT_PUBLISHER}"
  CreateShortCut "$SMPROGRAMS\${PRODUCT_PUBLISHER}\Uninstall.lnk" "$INSTDIR\uninstall.exe"
  CreateShortCut "$SMPROGRAMS\${PRODUCT_PUBLISHER}\${APP_NAME}.lnk" "$INSTDIR\${APP_NAME}.exe"
  CreateShortCut "$SMPROGRAMS\${APP_NAME}.lnk" "$INSTDIR\${APP_NAME}.exe"
  CreateShortCut "$DESKTOP\${APP_NAME}.lnk" "$INSTDIR\${APP_NAME}.exe"
SectionEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninstall.exe"
  WriteRegStr ${PRODUCT_BUILD_ROOT_KEY} "${PRODUCT_BUILD_KEY}" "DisplayName" "${PRODUCT_NAME}"
  WriteRegStr ${PRODUCT_BUILD_ROOT_KEY} "${PRODUCT_BUILD_KEY}" "VersionName" "${VERSION_NAME}"
  WriteRegStr ${PRODUCT_BUILD_ROOT_KEY} "${PRODUCT_BUILD_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"

	;NSIS 卸载程序的注册键值
	;http://blog.sina.com.cn/s/blog_407c173601007v2n.html
	WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "${PRODUCT_NAME} ${PRODUCT_VERSION}"
	WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "InstallLocation" "$INSTDIR"
	WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\UnInstall.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\config\img\install.ico"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
SectionEnd

/******************************
 *  以下是安装程序的卸载部分  *
 ******************************/

Section Uninstall

  Delete "$INSTDIR\uninstall.exe"
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Uninstall.lnk"
	Delete "$DESKTOP\${PRODUCT_NAME}.lnk"
	Delete "$SMPROGRAMS\${PRODUCT_PUBLISHER}\Uninstall.lnk"
	Delete "$SMPROGRAMS\${PRODUCT_PUBLISHER}\${PRODUCT_NAME}.lnk"
	Delete "$SMPROGRAMS\${PRODUCT_NAME}.lnk"
	Delete "$SMPROGRAMS\${PRODUCT_PUBLISHER}.lnk"
	Delete "$SMPROGRAMS\${PRODUCT_NAME}.lnk"
	Delete "$QUICKLAUNCH.lnk"
  Delete "$DESKTOP.lnk"

  RMDir "$SMPROGRAMS\${PRODUCT_PUBLISHER}"
  RMDir /r "${INSTALL_LOCATION}"
  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_BUILD_KEY}"
	DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  SetAutoClose true
SectionEnd

#-- 根据 NSIS 脚本编辑规则，所有 Function 区段必须放置在 Section 区段之后编写，以避免安装程序出现未可预知的问题。--#

Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "您确实要完全移除 $(^Name) ，及其所有的组件？" IDYES +2
  Abort
FunctionEnd

Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) 已成功地从您的计算机移除。"
FunctionEnd

