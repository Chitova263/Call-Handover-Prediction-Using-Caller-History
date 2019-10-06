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
    },
    chart:{
       margin: "0.5rem 0.5rem 0.5rem 0.5rem"
    },
    logo:{
      margin: "auto"
    }
  });


export default function GraphsPanel({data:results, isLoading, init }) {
    console.log(results, isLoading);
    const numOfCalls = [];
    const predictiveVHO = [];
    const nonPredictiveVHO = [];
    const handoversAvoided = [];
    if(!isLoading){
        results.forEach(result => {
            numOfCalls.push(result['calls']);
            predictiveVHO.push(result['predictiveHandovers']);
            nonPredictiveVHO.push(result['nonPredictiveHandovers']);
            handoversAvoided.push(result['nonPredictiveHandovers'] - result['predictiveHandovers']);
        })
    }

    const classes = useStyles();
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

    const avoided = {
      options: {
        colors: ['#000316'],
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
          data: [...handoversAvoided]
        }
      ]
    };  
    if(isLoading && !init){
        return<div className={classes.root}>
             <Spinner size={Spinner.SIZE_LARGE} />
        </div>
    }if(init){
      return <div className={classes.root}>
        <div className={classes.logo}>
          <img src={logo} alt="logo"/>
        </div>
      </div>
    }
    if(!init && !isLoading) {
        return (
            <div className={classes.root}>
                <Chart className={classes.chart}
                  options={data.options}
                  series={data.series}
                  type="bar"      
                />
                <Chart className={classes.chart}
                  options={avoided.options}
                  series={avoided.series}
                  type="bar"  
                />
            </div>
        )
    }  
}
