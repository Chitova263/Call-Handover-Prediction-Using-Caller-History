import React from 'react'
import { makeStyles } from '@material-ui/styles';
import Chart from "react-apexcharts";
import { Spinner } from '@blueprintjs/core';
import logo from './logo.png';

const useStyles = makeStyles({
    root: {
        display: "grid",
        gridTemplateColumns:"1fr 1fr",
        border: "2px solid gray",
        margin: "1rem 1rem 0",
        boxShadow: "inset 0 0 10px #000000", 
        height: "30rem",
        overflowY: "auto",    
    },
    chart:{
       margin: "0.5rem 0.5rem 0.5rem 0.5rem"
    },
    loading:{
      border: "2px solid gray",
      margin: "1rem 1rem 0",
      boxShadow: "inset 0 0 10px #000000", 
      height: "30rem",    
    },
    logo:{
      position: 'relative',
      top: "50%",
      left: "50%",
      transform: "translate(-50%, -50%)"
    }
  });


export default function GraphsPanel({data:results, isLoading, init }) {
  
    const classes = useStyles();  
  
    console.log(results, isLoading);

    const numOfCalls = [];
    const predictiveVHO = [];
    const nonPredictiveVHO = [];
    const handoversAvoided = [];
    const predictiveVoiceHandovers = [];
    const nonPredictiveVoiceHandovers = [];
    const voiceCalls = [];
    const predictiveDataHandovers = [];
    const nonPredictiveDataHandovers = [];
    const dataCalls = [];
    const voiceHandoversAvoided = [];
    const dataHandoversAvoided = [];

    if(!isLoading){
        results.forEach(result => {
            numOfCalls.push(result['calls']);
            predictiveVHO.push(result['predictiveHandovers']);
            nonPredictiveVHO.push(result['nonPredictiveHandovers']);
            handoversAvoided.push(result['nonPredictiveHandovers'] - result['predictiveHandovers']);
            predictiveVoiceHandovers.push(result['predictiveVoiceHandovers']);
            nonPredictiveVoiceHandovers.push(result['nonPredictiveVoiceHandovers']);
            voiceCalls.push(result['voiceCalls']);
            predictiveDataHandovers.push(result['predictiveDataHandovers']);
            nonPredictiveDataHandovers.push(result['nonPredictiveDataHandovers']);
            dataCalls.push(result['dataCalls']);
            voiceHandoversAvoided.push(result['nonPredictiveVoiceHandovers'] - result['predictiveVoiceHandovers']);
            dataHandoversAvoided.push(result['nonPredictiveDataHandovers'] - result['predictiveDataHandovers']);
        })
        console.log(predictiveDataHandovers, predictiveVoiceHandovers, voiceCalls, dataCalls)
        
    }



    //Extract this to a component
    const voice = {
      options: {
        colors: ['#125590', '#69b742'],
        chart: {
          id: "basic-bar",
        },
        legend: {
          onItemHover: {
            highlightDataSeries: true
          },
          onItemClick: {
              toggleDataSeries: true
          },
        },
        title: {
          text: "Number Of Voice Calls vs Number of Vertical handovers",
          align: 'center',
          margin: 10,
          offsetX: 0,
          offsetY: 0,
          floating: false,
          style: {
            fontSize:  '16px',
            color:  '#263238'
          },
      },
      yaxis:{
          title: {
              text: "Vertical Handovers"
          },
      },
        xaxis: {
          title: {
              text: "Number of Voice Calls"
          },
          labels: {
              show: true,
              rotate: -45,
              rotateAlways: false,
              hideOverlappingLabels: true,
              showDuplicates: false,
              trim: true,
              minHeight: undefined,
              maxHeight: 120,
              style: {
                  colors: [],
                  fontSize: '12px',
                  fontFamily: 'Helvetica, Arial, sans-serif',
                  cssClass: 'apexcharts-xaxis-label',
              },
          },
          categories: [...voiceCalls]
        }
      },
      series: [
        {
          
          name: "predictive algorithm",
          data: [...predictiveVoiceHandovers]
        },
        {
          name: "non predictive algorithm",
          data: [...nonPredictiveVoiceHandovers]
        }
      ]
    };

    const data = {
      options: {
        colors: ['#125590', '#69b742'],
        chart: {
          id: "basic-bar",
        },
        legend: {
          onItemHover: {
            highlightDataSeries: true
          },
          onItemClick: {
              toggleDataSeries: true
          },
        },
        title: {
          text: "Number Of Data Calls vs Number of Vertical handovers",
          align: 'center',
          margin: 10,
          offsetX: 0,
          offsetY: 0,
          floating: false,
          style: {
            fontSize:  '16px',
            color:  '#263238'
          },
      },
      yaxis:{
          title: {
              text: "Vertical Handovers"
          },
      },
        xaxis: {
          title: {
              text: "Number of Data Calls"
          },
          labels: {
              show: true,
              rotate: -45,
              rotateAlways: false,
              hideOverlappingLabels: true,
              showDuplicates: false,
              trim: true,
              minHeight: undefined,
              maxHeight: 120,
              style: {
                  colors: [],
                  fontSize: '12px',
                  fontFamily: 'Helvetica, Arial, sans-serif',
                  cssClass: 'apexcharts-xaxis-label',
              },
          },
          categories: [...dataCalls]
        }
      },
      series: [
        {
          
          name: "predictive algorithm",
          data: [...predictiveDataHandovers]
        },
        {
          name: "non predictive algorithm",
          data: [...nonPredictiveDataHandovers]
        }
      ]
    };
  
    const all = {
        options: {
          colors: ['#125590', '#69b742'],
          chart: {
            id: "basic-bar",
          },
          legend: {
            onItemHover: {
              highlightDataSeries: true
            },
            onItemClick: {
                toggleDataSeries: true
            },
          },
          title: {
            text: "Number Of Calls vs Number of Vertical handovers",
            align: 'center',
            margin: 10,
            offsetX: 0,
            offsetY: 0,
            floating: false,
            style: {
              fontSize:  '16px',
              color:  '#263238'
            },
        },
        yaxis:{
            title: {
                text: "Vertical Handovers"
            },
        },
          xaxis: {
            title: {
                text: "Number of Calls"
            },
            labels: {
                show: true,
                rotate: -45,
                rotateAlways: false,
                hideOverlappingLabels: true,
                showDuplicates: false,
                trim: true,
                minHeight: undefined,
                maxHeight: 120,
                style: {
                    colors: [],
                    fontSize: '12px',
                    fontFamily: 'Helvetica, Arial, sans-serif',
                    cssClass: 'apexcharts-xaxis-label',
                },
            },
            categories: [...numOfCalls]
          }
        },
        series: [
          {
            
            name: "predictive algorithm",
            data: [...predictiveVHO]
          },
          {
            name: "non predictive algorithm",
            data: [...nonPredictiveVHO]
          }
        ]
      };

      const dataAvoided = {
        options: {
          colors: ['#125590', '#69b742'],
          chart: {
            id: "basic-bar",
          },
          legend: {
            onItemHover: {
              highlightDataSeries: true
            },
            onItemClick: {
                toggleDataSeries: true
            },
          },
          title: {
            text: "Vertical Handovers Avoided For Requested Data Calls",
            align: 'center',
            margin: 10,
            offsetX: 0,
            offsetY: 0,
            floating: false,
            style: {
              fontSize:  '16px',
              color:  '#263238'
            },
        },
        yaxis:{
            title: {
                text: "Vertical Handovers Avoided"
            },
        },
          xaxis: {
            title: {
                text: "Number of Data Calls Requested"
            },
            labels: {
                show: true,
                rotate: -45,
                rotateAlways: false,
                hideOverlappingLabels: true,
                showDuplicates: false,
                trim: true,
                minHeight: undefined,
                maxHeight: 120,
                style: {
                    colors: [],
                    fontSize: '12px',
                    fontFamily: 'Helvetica, Arial, sans-serif',
                    cssClass: 'apexcharts-xaxis-label',
                },
            },
            categories: [...dataCalls]
          }
        },
        series: [
          {
            
            name: "predictive algorithm",
            data: [...dataHandoversAvoided]
          }
        ]
      };

      const voiceAvoided = {
        options: {
          colors: ['#125590', '#69b742'],
          chart: {
            id: "basic-bar",
          },
          legend: {
            onItemHover: {
              highlightDataSeries: true
            },
            onItemClick: {
                toggleDataSeries: true
            },
          },
          title: {
            text: "Vertical Handovers Avoided For Requested Voice Calls",
            align: 'center',
            margin: 10,
            offsetX: 0,
            offsetY: 0,
            floating: false,
            style: {
              fontSize:  '16px',
              color:  '#263238'
            },
        },
        yaxis:{
            title: {
                text: "Vertical Handovers Avoided"
            },
        },
          xaxis: {
            title: {
                text: "Number of Requested Voice Calls"
            },
            labels: {
                show: true,
                rotate: -45,
                rotateAlways: false,
                hideOverlappingLabels: true,
                showDuplicates: false,
                trim: true,
                minHeight: undefined,
                maxHeight: 120,
                style: {
                    colors: [],
                    fontSize: '12px',
                    fontFamily: 'Helvetica, Arial, sans-serif',
                    cssClass: 'apexcharts-xaxis-label',
                },
            },
            categories: [...voiceCalls]
          }
        },
        series: [
          {
            
            name: "predictive algorithm",
            data: [...voiceHandoversAvoided]
          }
        ]
      };

    const avoided = {
      options: {
        colors: ['#125590', '#69b742'],
        chart: {
          id: "basic-bar",
        },
        legend: {
          onItemHover: {
            highlightDataSeries: true
          },
          onItemClick: {
              toggleDataSeries: true
          },
        },
        title: {
          text: "Vertical Handovers Avoided",
          align: 'center',
          margin: 10,
          offsetX: 0,
          offsetY: 0,
          floating: false,
          style: {
            fontSize:  '16px',
            color:  '#263238'
          },
      },
      yaxis:{
          title: {
              text: "Vertical Handovers Avoided"
          },
      },
        xaxis: {
          title: {
              text: "Number of Calls"
          },
          labels: {
              show: true,
              rotate: -45,
              rotateAlways: false,
              hideOverlappingLabels: true,
              showDuplicates: false,
              trim: true,
              minHeight: undefined,
              maxHeight: 120,
              style: {
                  colors: [],
                  fontSize: '12px',
                  fontFamily: 'Helvetica, Arial, sans-serif',
                  cssClass: 'apexcharts-xaxis-label',
              },
          },
          categories: [...numOfCalls]
        }
      },
      series: [
        {
          
          name: "predictive algorithm",
          data: [...handoversAvoided]
        }
      ]
    };

    if(isLoading && !init){
        return<div className={classes.loading}>
             <Spinner size={Spinner.SIZE_LARGE} className={classes.logo}/>
        </div>
    }if(init){
      return <div >
        <div className={classes.loading}>
          <img src={logo} alt="logo" className={classes.logo}/>
        </div>
      </div>
    }
    if(!init && !isLoading) {
        return (
            <div className={classes.root}>
                <Chart className={classes.chart}
                  options={all.options}
                  series={all.series}
                  type="bar"      
                />
                <Chart className={classes.chart}
                  options={avoided.options}
                  series={avoided.series}
                  type="bar"  
                />
                <Chart className={classes.chart}
                  options={voice.options}
                  series={voice.series}
                  type="bar"  
                />
                <Chart className={classes.chart}
                  options={voiceAvoided.options}
                  series={voiceAvoided.series}
                  type="bar"  
                />
                <Chart className={classes.chart}
                  options={data.options}
                  series={data.series}
                  type="bar"  
                />
                <Chart className={classes.chart}
                  options={dataAvoided.options}
                  series={dataAvoided.series}
                  type="bar"  
                />
            </div>
        )
    }  
}
