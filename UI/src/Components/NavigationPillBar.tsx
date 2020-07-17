import './NavigationPillBar.scss';
import React, { useState } from 'react';
import NavigationPill from './NavigationPill';

type NavigationPillBarProps = {
  pills: any
}

function NavigationPillBar(props: NavigationPillBarProps & React.HTMLProps<HTMLDivElement>) {
  const {pills, className, ...others} = props;
  const [navPills, setNavPills] = useState(pills || []);

  const setActivePill = (index: number) => {
    var tempPills = navPills.map((x: any) => {x.active = false; return x;});
    tempPills[index].active = true;
    setNavPills(tempPills);
  }

  return (
    <div className={"pillbar__wrapper full-width " + className} {...others}>
      {
        navPills.map((pill:any, index:any) => 
          <NavigationPill key={index} index={index} onClick={() => setActivePill(index)} {...pill}/>
        )
      }
    </div>
  )
}

export default NavigationPillBar;