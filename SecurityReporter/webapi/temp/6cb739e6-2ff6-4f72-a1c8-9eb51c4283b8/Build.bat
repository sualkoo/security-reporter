@REM
@REM		File:		Build.bat
@REM		Author: 	Dusan Repel (SHS TE DC CYS CSA)
@REM		Date:		02/09/2020
@REM
@REM
@ECHO Recompile Title Page, included as pdf page
cd TitlePage
call build.bat
cd ..
call clean.bat
@ECHO Compile LaTeX + build Bibliography + fix cross-references
pdflatex Main
bibtex   Main
pdflatex Main
pdflatex Main

