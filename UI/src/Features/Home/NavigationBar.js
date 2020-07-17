import React, { useEffect } from 'react';
import './NavigationBar.scss';
import NavigationItem from './NavigationItem';
import { useHistory, useLocation } from 'react-router-dom';

export default function NavigationBar(props) {
  const {items, setItems, toggle, ...others} = props;
  let history = useHistory();
  const {pathname} = useLocation();

  const setActiveItem = (index) => {
    var tempItems = items.map((x) => {x.active = false; return x;});
    tempItems[index].active = true;
    setItems(tempItems);
    toggle();
    history.push(tempItems[index].route);
  }

  useEffect(() => {
    var activeIndex = items.findIndex((val) => pathname.includes(val.route));
    if(activeIndex >= 0) {
      var tempItems = items.map((x => {x.active = false; return x;}));
      tempItems[activeIndex].active = true;
      setItems(tempItems);
    }
  }, [pathname])

  return (
    <div className="navbar__content flex-column">
      {
        items.map((item, index) => 
          <NavigationItem key={index} index={index} onClick={() => setActiveItem(index)} {...item} />
        )
      }
    </div>
  );
}
