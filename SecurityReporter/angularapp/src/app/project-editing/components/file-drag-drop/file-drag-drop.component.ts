import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { FileDownloadService } from '../../services/file-download.service';
import { UploadService } from '../../services/upload.service';

@Component({
  selector: 'app-file-drag-drop',
  templateUrl: './file-drag-drop.component.html',
  styleUrls: ['./file-drag-drop.component.css'],
  standalone: true,
  imports: [CommonModule, MatIconModule],
})
export class FileDragDropComponent {
  uploadedFiles: File[] = [];
  constructor(private fileDownloadService: FileDownloadService, private uploadService: UploadService) { }

  @Input() pentestTitle?: string;
  @Input() id: string = "";
  @Input() pentest: string = "";


  downloadFile(fileName: string) {
    this.fileDownloadService.getDownloadFile(fileName)
      .then((response: Blob) => {
        const blob = new Blob([response], { type: 'application/pdf' });
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = fileName;
        a.click();
        window.URL.revokeObjectURL(url);
      })
      .catch((error) => {
        console.error(error);
      });
  }


  onFileSelected(event: any) {
    const files: FileList = event.target.files;
    this.processFiles(files);
  }

  onDrop(event: any) {
    event.preventDefault();
    const files: FileList = event.dataTransfer.files;
    this.processFiles(files);
  }

  onDragOver(event: any) {
    event.preventDefault();
  }

  upload(file: File, destination: string) {
    this.uploadService.upload(file, destination, this.id);
  }

  private processFiles(files: FileList) {
    this.uploadedFiles = [];
    for (let i = 0; i < files.length; i++) {
      this.uploadedFiles.push(files[i]);
    }




  }
}
