import React from 'react'
import { makeStyles } from '@material-ui/styles';
import Chart from "react-apexcharts";
import { Divider, Spinner } from '@blueprintjs/core';

const useStyles = makeStyles({
    root: {
        display: "flex",
        justifyContent: "space-around",
        border: "2px solid black",
        margin: "1rem 1rem 0",
        boxShadow: "inset 0 0 10px #000000", 
        height: "377px",    
    },
    chart:{
        margin: "1.5rem"
    }
  });


export default function GraphsPanel({data:results, isLoading }) {
    console.log(results, isLoading);
    const numOfCalls = [];
    const predictiveVHO = [];
    const nonPredictiveVHO = [];
    if(!isLoading){
        results.forEach(result => {
            numOfCalls.push(result['calls']);
            predictiveVHO.push(result['predictiveHandovers']);
            nonPredictiveVHO.push(result['nonPredictiveHandovers']);
        })
    }
    
    console.log(nonPredictiveVHO)

    const classes = useStyles();
    const data = {
        options: {
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
    if(isLoading){
        return<div className={classes.root}>
             <Spinner size={Spinner.SIZE_LARGE} />
        </div>
    }else{
        return (
            <div className={classes.root}>
                <Chart className={classes.chart}
                  options={data.options}
                  series={data.series}
                  type="line"
                  width="500"
                />
                <Divider/>
            </div>
        )
    }  
}
