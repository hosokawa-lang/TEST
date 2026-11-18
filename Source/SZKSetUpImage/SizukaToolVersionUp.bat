@echo OFF

set "BASEDIR=%~dp0"
set LOGFILE=%~dp0batchlog_%date:~-10,4%%date:~-5,2%%date:~-2,2%_%time:~0,2%%time:~3,2%%time:~6,2%.txt
set LOGFILE=%LOGFILE: =0%
echo ログ出力先: %LOGFILE%

:main
setlocal enabledelayedexpansion
echo ===============================
echo  SIZUKA Tool インストール/バージョンアップ  %date% %time%
echo ===============================
echo =============================== >> %LOGFILE%
echo  SIZUKA Tool インストール/バージョンアップ  %date% %time%  >> %LOGFILE%
echo =============================== >> %LOGFILE%
echo.
Echo #####  古バージョンの存在確認中...  #####
Echo #####  古バージョンの存在確認中...  ##### >> %LOGFILE%
set "OldFilePath=C:\Program Files\Cyber\Sizuka Tool\"
if exist "!OldFilePath!SZKTool.exe" (
	Echo 古バージョンの存在ありました。
	Echo 古バージョンの存在ありました。 >> %LOGFILE%
	Echo 古バージョンのアンインストールパラメータを検索中です... 
	Echo 古バージョンのアンインストールパラメータを検索中です... >> %LOGFILE%
	set "APPNAME=Sizuka Tool"
	:: PowerShell スクリプトのパス
	set "PS_SCRIPT=%~dp0AppUninstallerInfo.ps1"
	echo PS_SCRIPT: !PS_SCRIPT!
	echo PS_SCRIPT: !PS_SCRIPT!  >> %LOGFILE%
	:: PowerShell を管理者権限で呼び出してスクリプト実行
	REM powershell -ExecutionPolicy Bypass -File "!PS_SCRIPT!" -AppName "!APPNAME!"
	for /f "usebackq delims=" %%A in (`powershell -ExecutionPolicy Bypass -File "!PS_SCRIPT!" -AppName "!APPNAME!"`) do (
	    set "UNINSTALL=%%A"
	)	
	
	if !UNINSTALL!=="" (
	    echo 古バージョンのアンインストールパラメータが見つかりませんでした。
	    echo 古バージョンのアンインストールパラメータが見つかりませんでした。 >> %LOGFILE%
	    GOTO NewInstall
	) Else (
		REM 古バージョンのDeviceList.iniのバックアップ >> %LOGFILE%
		copy /B "!OldFilePath!DeviceList.ini" "%~dp0"
		echo 古バージョンのDeviceList.iniのバックアップ完了
		echo 古バージョンのDeviceList.iniのバックアップ完了 >> %LOGFILE%
	)

	echo 古バージョンのアンインストールを開始します
	echo 古バージョンのアンインストールを開始します >> %LOGFILE%
	:: /I を /X に置換して /qn /norestart を追加
	set "UNINSTALL_SILENT=!UNINSTALL:/I=/X! /qn /norestart"
	echo  Uninstall command: !UNINSTALL_SILENT!
	echo  Uninstall command: !UNINSTALL_SILENT! >> %LOGFILE%
	echo 古バージョンのアンインストール中...
	echo 古バージョンのアンインストール中... >> %LOGFILE%
	!UNINSTALL_SILENT!
	echo 古バージョンのアンインストールを完了しました。
	echo 古バージョンのアンインストールを完了しました。 >> %LOGFILE%
) else (
	Echo 古バージョンのアンインストールは必要ありませんでした。
	Echo 古バージョンのアンインストールは必要ありませんでした。 >> %LOGFILE%
)


:NowVersionUninstall
	Echo ######  アップデートを開始します  ######
	Echo ######  アップデートを開始します  ######  >> %LOGFILE%
	Echo アンインストールパラメータを検索中です...
	Echo アンインストールパラメータを検索中です...  >> %LOGFILE%
	set "APPNAME=Sizuka Tool"
	set "NewInstall=C:\SZKApps\Sizuka Tool\"
	if  exist "!NewInstall!SZKTool.exe" (
		:: PowerShell スクリプトのパス
		set "PS_SCRIPT=%~dp0AppUninstallerInfo.ps1"
		:: PowerShell を管理者権限で呼び出してスクリプト実行
		REM powershell -ExecutionPolicy Bypass -File "!PS_SCRIPT!" -AppName "!APPNAME!"
		for /f "usebackq delims=" %%A in (`powershell -ExecutionPolicy Bypass -File "!PS_SCRIPT!" -AppName "!APPNAME!"`) do (
		    set "UNINSTALL=%%A"
		)	
		echo UninstallString = !UNINSTALL!
		echo UninstallString = !UNINSTALL! >> %LOGFILE%
		if !UNINSTALL!=="" (
		    echo 新インストール場所のアンインストールパラメータが見つかりませんでした。
		    echo 新インストール場所のアンインストールパラメータが見つかりませんでした。 >> %LOGFILE%
		    GOTO NewInstall
		) else (
			REM 前バージョンのDeviceList.iniのバックアップ
			if  exist "!NewInstall!DeviceList.ini" (
				copy /B "!NewInstall!DeviceList.ini" "%~dp0" 
				echo 複写 copy /B "!NewInstall!DeviceList.ini" "%~dp0"
				echo 複写 copy /B "!NewInstall!DeviceList.ini" "%~dp0"  >> %LOGFILE%
				echo 前バージョンのDeviceList.iniのバックアップ完了
				echo 前バージョンのDeviceList.iniのバックアップ完了 >> %LOGFILE%
			)
		)
		:: /I を /X に置換して /qn /norestart を追加
		set "UNINSTALL_SILENT=!UNINSTALL:/I=/X! /qn /norestart"
		echo  Uninstall command: !UNINSTALL_SILENT!
		echo  Uninstall command: !UNINSTALL_SILENT! >> %LOGFILE%
		echo 新場所バージョンのアンインストール中...
		echo 新場所バージョンのアンインストール中...  >> %LOGFILE%
		!UNINSTALL_SILENT! 
		echo 新場所バージョンのアンインストールを完了しました。
		echo 新場所バージョンのアンインストールを完了しました。 >> %LOGFILE%
	) else (
		Echo 新場所バージョンのアンインストールは必要ありませんでした。
		Echo 新場所バージョンのアンインストールは必要ありませんでした。 >> %LOGFILE%
	)

:NewInstall
	echo インストールを開始します...
	echo インストールを開始します...  >> %LOGFILE%
	msiexec.exe /i "%~dp0Setupfiles\SizukaSetup.msi" /qn /norestart
	REM %~dp0Setupfiles\Setup.exe
	echo インストールを完了しました。
	echo インストールを完了しました。 >> %LOGFILE%
	
	If exist "%~dp0DeviceList.ini" (
		copy /B "%~dp0DeviceList.ini" "!NewInstall!"
		echo 複写 copy /B %~dp0DeviceList.ini
		echo 複写 copy /B %~dp0DeviceList.ini !NewInstall! >> %LOGFILE%
		REM del %~dp0DeviceList.ini
		REM echo del %~dp0DeviceList.ini
		echo DeviceList.iniの移植完了
		echo DeviceList.iniの移植完了 >> %LOGFILE%
	)
	
	Echo ######  アップデートを終了します  ######   %date% %time%
	Echo ######  アップデートを終了します  ######   %date% %time% >> %LOGFILE%
endlocal
pause

