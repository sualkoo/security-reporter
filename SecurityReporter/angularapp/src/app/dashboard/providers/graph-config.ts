import { ChartOptions } from 'chart.js'
export const CriticalityConfig = {
  type: 'pie',
  label: '% of found Criticality',
  backgroundColors: [
    'rgb(217,0,45)',
    'rgb(0,151,255)',
    'rgb(253,179,0)',
    'rgb(102,255,0)',
    'rgb(153,102,255)',
  ],
  borderWidth: 0,
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
        position: 'right'as 'right',

        labels: {
          color: 'black',
          font: {
            size: 14,
            weight: 'bold'

          },
          usePointStyle: true,
          boxWidth: 10,
          padding: 20

        },
      },
    },
  } as ChartOptions,
};
