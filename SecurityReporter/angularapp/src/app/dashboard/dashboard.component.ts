import { Component, OnInit } from '@angular/core';
import { Chart } from "chart.js/auto";
import { DashboardService } from './providers/dashboard-service';
import { switchMap } from "rxjs";
import {CriticalityConfig, VulnerabilityConfig} from "./providers/graph-config";

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  criticality: any[] = [];
  criticalityChart: any;

  vulnerability: any[] = [];
  vulnerabilityChart: any;
  constructor(private dashboardService: DashboardService) {

  }

  ngOnInit(): void {
    this.dashboardService.getCriticality()
      .pipe(
        switchMap((criticalityData: any[]) => {
          this.criticality = criticalityData;
          this.updateCriticalityChart();
          return this.dashboardService.getVulnerability();
        })
      )
      .subscribe((vulnerabilityData: any[]) => {
        this.vulnerability = vulnerabilityData;
        this.updateVulnerabilityChart();
      });
  }

  forLoopInjection(data:any, number: any, percentage:any, labels:any){
    number = data.reduce((accumulator:any, currentValue:any) => accumulator + currentValue, 0);
    for (let i = 0; i < data.length; i++) {
      percentage[i] = Math.round((data[i] / number) * 100);
      labels[i] = "Level: " + labels[i];
    }
    return {percentage, labels, number}
  }

  updateCriticalityChart(): void {
    //sort from smallest to largest
    this.criticality.sort((a, b) => a.item1 - b.item1);

    let labels = this.criticality.map((item) => item.item1);
    const data = this.criticality.map((item) => item.item2);
    let number = 0;
    let percentage = [null];

    let object = this.forLoopInjection(data, number, percentage, labels)
    labels = object.labels;
    percentage = object.percentage;
    number = object.number;

    if (this.criticalityChart) {
      this.criticalityChart.data.labels = labels;
      this.criticalityChart.data.datasets[0].data = percentage;
      this.criticalityChart.update();
    } else {
      let ctx = (document.getElementById('criticalityChart') as HTMLCanvasElement).getContext('2d');
      if (ctx) {
        // @ts-ignore
        this.criticalityChart = new Chart(ctx, {
          type: CriticalityConfig.type,
          data: {
            labels: labels,
            datasets: [{
              label: CriticalityConfig.label,
              data: percentage,
              backgroundColor: CriticalityConfig.backgroundColors,
              borderWidth: CriticalityConfig.borderWidth,
            }],
          },
          options: CriticalityConfig.options, // Move options out of the data object
        });
      }
    }

  }


  updateVulnerabilityChart(): void {
    this.vulnerability.sort((a, b) => a.item1 - b.item1);
    let labels = this.vulnerability.map((item) => item.item1);
    const data = this.vulnerability.map((item) => item.item2);
    let number = 0;
    let percentage =[null];

    let object = this.forLoopInjection(data, number, percentage, labels)
    labels = object.labels;
    percentage = object.percentage;
    number = object.number;

    if (this.vulnerabilityChart) {
      this.vulnerabilityChart.data.labels = labels;
      this.vulnerabilityChart.data.datasets[0].data = percentage;
      this.vulnerabilityChart.update();
    } else {
      let ctx = (document.getElementById('vulnerabilityChart') as HTMLCanvasElement).getContext('2d');
      if(ctx){
        this.vulnerabilityChart = new Chart(ctx, {
        type: VulnerabilityConfig.type,
        data: {
          labels: labels,
          datasets: [{
            label: VulnerabilityConfig.label,
            data: percentage,
            backgroundColor: VulnerabilityConfig.backgroundColors,
            borderWidth: VulnerabilityConfig.borderWidth,
          }],
        },
          options: VulnerabilityConfig.options,
      });
    }}
  }

  updateCWEChart(): void {
    console.log('updateCWEChart')
  }
}
