import { Component, OnInit } from '@angular/core';
import { Chart } from "chart.js/auto";
import { DashboardService } from './providers/dashboard-service';
import { switchMap } from "rxjs";
import {CriticalityConfig, CWEConfig, VulnerabilityConfig} from "./providers/graph-config";

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

  cwe: any[] = [];
  cweChart: any;
  constructor(private dashboardService: DashboardService) {

  }

  ngOnInit(): void {
    this.dashboardService.getCriticality()
      .pipe(
        switchMap((criticalityData: any[]) => {
          this.criticality = criticalityData;
          this.updateCriticalityChart();
          return this.dashboardService.getVulnerability();
        }),
        switchMap((vulnerabilityData: any[]) => {
          this.vulnerability = vulnerabilityData;
          this.updateVulnerabilityChart();
          return this.dashboardService.getCWE();
        }
      ))
      .subscribe((cwe: any[]) => {
        this.cwe = cwe;
        this.updateCWEChart();
      })


  }

  forLoopInjection(data: any, sumOfValues: any, percentage:any, labels:any){
    sumOfValues = data.reduce((accumulator:any, currentValue:any) => accumulator + currentValue, 0);
    for (let i = 0; i < data.length; i++) {
      percentage[i] = Math.round((data[i] / sumOfValues) * 100);
      labels[i] = labels[i];
    }
    return { percentage, labels, sumOfValues }
  }

  updateCriticalityChart(): void {
    this.criticality.sort((a, b) => a.item3 - b.item3);

    let labels = this.criticality.map((item) => item.item1);
    const data = this.criticality.map((item) => item.item2);
    let sumOfValues = 0;
    let percentage = [null];

    let object = this.forLoopInjection(data, sumOfValues, percentage, labels)
    labels = object.labels;
    percentage = object.percentage;
    sumOfValues = object.sumOfValues;

      let ctx = (document.getElementById('criticalityChart') as HTMLCanvasElement).getContext('2d');
      if (ctx) {
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
          options: CriticalityConfig.options,
        });
      }


  }


  updateVulnerabilityChart(): void {
    this.vulnerability.sort((a, b) => a.item3 - b.item3);
    let labels = this.vulnerability.map((item) => item.item1);
    const data = this.vulnerability.map((item) => item.item2);
    let sumOfValues = 0;
    let percentage =[null];

    let object = this.forLoopInjection(data, sumOfValues, percentage, labels)
    labels = object.labels;
    percentage = object.percentage;
    sumOfValues = object.sumOfValues;

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
    }
  }


  updateCWEChart(): void {
    this.cwe.sort((a, b) => a.item2 - b.item2);

    let labels = this.cwe.map((item) => item.item2);
    const data = this.cwe.map((item) => item.item1);
    let sumOfValues = 0;
    let percentage = [null];

    let object = this.forLoopInjection(data, sumOfValues, percentage, labels)
    labels = object.labels;
    percentage = object.percentage;
    sumOfValues = object.sumOfValues;

    let ctx = (document.getElementById('CWEChart') as HTMLCanvasElement).getContext('2d');
    if (ctx) {
      this.cweChart = new Chart(ctx, {
        type: CWEConfig.type,
        data: {
          labels: labels,
          datasets: [{
            label:"CWE contains",
            data: data,
            backgroundColor: CWEConfig.backgroundColors,
            borderWidth: CWEConfig.borderWidth,
          }],
        },
        options: CWEConfig.options,
      });
    }


  }
}
