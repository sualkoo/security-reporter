import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SharingDataService {

  constructor() { }

  private data: string[] = [];

    setData(data: string[]) {
        this.data = data;
    }

    getData(): string[] {
        return this.data;
    }
}
