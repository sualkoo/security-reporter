import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';

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

  private processFiles(files: FileList) {
    for (let i = 0; i < files.length; i++) {
      this.uploadedFiles.push(files[i]);
    }
  }
}
