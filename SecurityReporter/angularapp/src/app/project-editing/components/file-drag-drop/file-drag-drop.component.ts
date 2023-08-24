import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
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

  @Input() pentestTitle?: string;
  @Input() id: string = "";
  @Input() pentest: string = "";

  constructor(private uploadService: UploadService) { }

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
    var ret = this.uploadService.upload(file, destination, this.id);
    // TODO handle return - alertservice + vypisat co vracia
    // TODO css + aby iba jeden mohol byt + ostatne buttons
  }

  private processFiles(files: FileList) {
    for (let i = 0; i < files.length; i++) {
      this.uploadedFiles.push(files[i]);
    }
  }
}
