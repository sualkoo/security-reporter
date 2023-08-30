## Issue 1: Upload Functionality and PDF Generation

- **Description:**
  The upload functionality is currently operational only on Linux systems. It requires the installation of `texlive-full` for proper functionality. While the current implementation relies on Linux system commands, it has the potential to be extended to Windows. Additionally, the generation of PDFs is not yet supported. In case of PDF generation with reportbuilder image, we have kept a method for it.
- **Status:** Open
- **Related:** User Story 1613 (incomplete), `webapi/ProjectSearch/Services/PDFBuilder.cs`

## Issue 2: Project Search Page Responsiveness

- **Description:**
  The Project Search page lacks full responsiveness due to a fixed sidebar configuration. As a result, when scrolling vertically, an empty space appears above the sidebar.
- **Status:** Open
- **Related:** `Project Search` page (angularapp/src/app/project-search/component-pages/project-search-page)

## Issue 3: LaTeX Commands in Findings Descriptions

- **Description:**
  Descriptions within the Findings section may occasionally include LaTeX commands, although the majority of them are filtered out.
- **Status:** Open
- **Related:** `ProjectDataParser.cs` and it's subclasses - `webapi/ProjectSearch/Services/Extractor/ZipToDBExtractor`. Note that we also parse information from DB back to the ZIP, so you need to adjust these at `DBToZipExtractor`.
