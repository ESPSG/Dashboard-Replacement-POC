import React from 'react';
import './Header.scss';
import HamburgerMenu from 'react-hamburger-menu';

export default function Header(props) {
  const {toggleSidebar, isSidebarOpen, ...others} = props;

  return (
    <div className="header__content flex-vertical-center full-size">
      <HamburgerMenu 
        className="header__hamburger" menuClicked={toggleSidebar} 
        isOpen={isSidebarOpen} color="white" animationDuration={0.2} 
        strokeWidth={3} width={25} height={18}/>
      <h3>
        <a href="/" className="color-light">
          Ed-Fi Dashboard
        </a>
      </h3>
    </div>
  );
}
