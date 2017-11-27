@echo off

::Proto�ļ�·��
set SOURCE_PATH=.\Proto

::Protogen����·��
set PROTOGEN_PATH=..\ProtoGen\protogen.exe
::C#�ļ�����·��
set TARGET_PATH=.\Cs

::ɾ��֮ǰ�������ļ�
del %TARGET_PATH%\*.* /f /s /q
echo -------------------------------------------------------------

for /f "delims=" %%i in ('dir /b "%SOURCE_PATH%\*.proto"') do (
    
    echo ת����%%i to %%~ni.cs
    %PROTOGEN_PATH% -i:%SOURCE_PATH%\%%i -o:%TARGET_PATH%\%%~ni.cs -ns:ProtoData
    
)

echo ת�����

pause