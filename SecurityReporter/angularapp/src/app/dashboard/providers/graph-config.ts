import {TitleOptions, ChartOptions, ChartType} from 'chart.js';
export const CriticalityConfig = {
  type: 'pie' as ChartType,
  label: '% of found Criticality',
  backgroundColors: [
    "#ec6602", // Healthy Orange
    "#009999", // Siemens Petrol
    "#9ddff6", // SH Cyan(50)
    "#c69b9e", // SH Berry(50)
    "#9592c1", // SH Blue(50)
    "#666666", // SH Black(60)
  ],
  borderWidth: 1,
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


export const VulnerabilityConfig = {
  type: 'doughnut' as ChartType,
  label: '% of found Vulnerability',
  backgroundColors: [
    "#ec6602", // Healthy Orange
    "#009999", // Siemens Petrol
    "#9ddff6", // SH Cyan(50)
    "#c69b9e", // SH Berry(50)
    "#9592c1", // SH Blue(50)
    "#666666", // SH Black(60)
  ],
  borderWidth: 0,
  options: {
    plugins: {
      responsive: true,
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
  }as ChartOptions
};


export const CWEConfig = {
  type: 'bar' as ChartType,
  label:{
    display: false,
  },
  backgroundColors: [
    "#ec6602", // Healthy Orange
    "#009999", // Siemens Petrol
    "#9ddff6", // SH Cyan(50)
    "#c69b9e", // SH Berry(50)
    "#9592c1", // SH Blue(50)
    "#666666", // SH Black(60)
  ],
  borderWidth: 0,
  options: {
    plugins: {
      responsive: true,
      title: {
        display: true,
        text: 'CWEs leaderboard',
        color: 'black',
        align: 'center',
        font: {
          size: 20,
        }
      },
      legend: {
        display: false, // Hide the default legend,

      },
    }
  }as ChartOptions
};

export const CVSSConfig = {
  type: 'bar' as ChartType,
  label:{
    display: false,
  },
  backgroundColors: [
    "#ec6602", // Healthy Orange
    "#009999", // Siemens Petrol
    "#9ddff6", // SH Cyan(50)
    "#c69b9e", // SH Berry(50)
    "#9592c1", // SH Blue(50)
    "#666666", // SH Black(60)
  ],
  borderWidth: 0,
  options: {
    plugins: {
      responsive: true,
      title: {
        display: true,
        text: 'Average Report CVSS score per month',
        color: 'black',
        align: 'center',
        font: {
          size: 20,
        }
      },
      legend: {
        display: false, // Hide the default legend,

      },
    }
  }as ChartOptions
};
