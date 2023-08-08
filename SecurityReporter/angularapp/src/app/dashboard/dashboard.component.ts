import { Component, OnInit } from '@angular/core';
import {Chart} from "chart.js/auto";
import { DashboardService } from './providers/dashboard-service';
import {switchMap} from "rxjs";
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

  updateCriticalityChart(): void {
    //sort from smallest to largest
    this.criticality.sort((a, b) => a.item1 - b.item1);

    let labels = this.criticality.map((item) => item.item1);
    const data = this.criticality.map((item) => item.item2);
    let number = 0;
    let percentage =[];
    for (let i = 0; i < data.length; i++) {
      number += data[i];
    }
    for (let i = 0; i < data.length; i++) {
      percentage[i] = Math.round((data[i]/number)*100);
      labels[i] = "Level: " + labels[i];
    }

    if (this.criticalityChart) {
      this.criticalityChart.data.labels = labels;
      this.criticalityChart.data.datasets[0].data = percentage;
      this.criticalityChart.update();
    } else {
      let ctx = (document.getElementById('criticalityChart') as HTMLCanvasElement).getContext('2d');
      if(ctx){
        // @ts-ignore
        this.criticalityChart = new Chart(ctx, {
          type: 'pie',
          data: {
            labels: labels,
            datasets: [{
              label: "# of Votes",
              data: percentage,

              backgroundColor: [
                'rgb(217,0,45)',
                'rgb(0,151,255)',
                'rgb(253,179,0)',
                'rgb(102,255,0)',
                'rgb(153,102,255)',
              ],
              borderWidth: 0,
            }],

          },
          options: {
            plugins: {
              title: {
                display: true,
                text: 'Overall Findings Criticality',
                color: 'black',
                align: 'center',
                font: {
                  size: 20,
                }
              },
              legend: {
                display: true, // Hide the default legend,
                align: 'center',
                position: 'right',

                labels: {
                  color: 'black',
                  font: {
                    size: 14,
                    weight: 'bold'

                  },
                  usePointStyle: true,
                  boxWidth: 10,
                  padding: 20

                }
              },
            }
          }
        });
      }}
  }


  updateVulnerabilityChart(): void {
    //sort from smallest to largest
    this.vulnerability.sort((a, b) => a.item1 - b.item1);

    let labels = this.vulnerability.map((item) => item.item1);
    const data = this.vulnerability.map((item) => item.item2);
    let number = 0;
    let percentage =[];
    for (let i = 0; i < data.length; i++) {
      number += data[i];
    }
    for (let i = 0; i < data.length; i++) {
      percentage[i] = Math.round((data[i]/number)*100);
      labels[i] = "Level: " + labels[i];
    }

    if (this.vulnerabilityChart) {
      this.vulnerabilityChart.data.labels = labels;
      this.vulnerabilityChart.data.datasets[0].data = percentage;
      this.vulnerabilityChart.update();
    } else {
      let ctx = (document.getElementById('vulnerabilityChart') as HTMLCanvasElement).getContext('2d');
      if(ctx){
      // @ts-ignore
        this.vulnerabilityChart = new Chart(ctx, {
        type: 'pie',
        data: {
          labels: labels,
          datasets: [{
            label: "# of Votes",
            data: percentage,

            backgroundColor: [
              'rgb(217,0,45)',
              'rgb(0,151,255)',
              'rgb(253,179,0)',
              'rgb(102,255,0)',
              'rgb(153,102,255)',
            ],
            borderWidth: 0,
          }],

        },
          options: {
          plugins: {

            title: {
              display: true,
              text: 'Most Reported Vulnerabilities',
              color: 'black',
              align: 'center',
              font: {
                size: 20,
              }
            },
            legend: {
              display: true, // Hide the default legend,
              align: 'center',
              position: 'right',

              labels: {
                color: 'black',
                font: {
                  size: 14,
                  weight: 'bold'

                },
              usePointStyle: true,
              boxWidth: 10,
              padding: 20
              }
            },
          }
        }
      });
    }}
  }

  updateCWEChart(): void {
    console.log('updateCWEChart')
  }
}
