@REM Create container if it doesn't exist yet
docker run -itd --name reportbuilder reportbuilder:1.0

@REM Start the container if it's not running yet
docker start reportbuilder

@REM Copy sources to docker
docker cp ./. reportbuilder:/data

@REM Compile the sources
docker exec -it reportbuilder pdflatex Main
docker exec -it reportbuilder pdflatex Main
docker exec -it reportbuilder bibtex Main
docker exec -it reportbuilder pdflatex Main

@REM Extract the compiled PDF
docker cp reportbuilder:/data/Main.pdf .

@REM Stop the container running in the background 
@REM docker stop reportbuilder