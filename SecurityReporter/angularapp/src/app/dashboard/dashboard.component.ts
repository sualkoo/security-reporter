import { Component, OnInit } from '@angular/core';
import * as Chart from 'chart.js';
import { DashboardService } from './providers/dashboard-service';

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
      .subscribe((data: any[]) => {
        this.criticality = data;
        this.updateCriticalityChart();
        this.dashboardService.getVulnerability().subscribe(
          (data: any[]) => {
            this.vulnerability = data;
            this.updateVulnerabilityChart();
          }
        )
      })
  }

  updateCriticalityChart(): void {
    const labels = this.criticality.map((item) => item.item1);
    const data = this.criticality.map((item) => item.item2);
    if (this.criticalityChart) {
      this.criticalityChart.data.labels = labels;
      this.criticalityChart.data.datasets[0].data = data;
      this.criticalityChart.update();
    } else {
      this.criticalityChart = new Chart('criticalityChart', {
        type: 'pie',
        data: {
          labels: labels,
          datasets: [{
            label: '# of Votes',
            data: data,
            backgroundColor: [
              'rgb(255,99,132)',
              'rgb(54,162,235)',
              'rgb(255,206,86)',
              'rgb(75,192,192)',
              'rgb(153,102,255)',
            ],
            borderColor: [
              'rgb(255,99,132)',
              'rgb(54,162,235)',
              'rgb(255,206,86)',
              'rgb(75,192,192)',
              'rgb(153,102,255)',
            ],
            borderWidth: 1
          }]
        },
        options: {
          // ... your options
        }
      });
    }
  }
  updateVulnerabilityChart(): void {
    const labels = this.vulnerability.map((item) => item.item1);
    const data = this.vulnerability.map((item) => item.item2);
    if (this.vulnerabilityChart) {
      this.vulnerabilityChart.data.labels = labels;
      this.vulnerabilityChart.data.datasets[0].data = data;
      this.vulnerabilityChart.update();
    } else {
      this.vulnerabilityChart = new Chart('vulnerabilityChart', {
        type: 'pie',
        data: {
          labels: labels,
          datasets: [{
            label: '# of Votes',
            data: data,
            backgroundColor: [
              'rgb(217,0,45)',
              'rgb(0,151,255)',
              'rgb(253,179,0)',
              'rgb(102,255,0)',
              'rgb(153,102,255)',
            ],
            borderColor: [
              'rgb(217,0,45)',
              'rgb(0,151,255)',
              'rgb(253,179,0)',
              'rgb(102,255,0)',
              'rgb(153,102,255)',
            ],
            borderWidth: 1
          }]
        },
        options: {
          // ... your options
        }
      });
    }
  }
}
