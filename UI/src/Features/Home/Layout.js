import "./Layout.scss";
import React, { useState } from 'react';
import Footer from './Footer';
import NavigationBar from './NavigationBar';
import Header from "./Header";
import { toggledClass, formatRoute } from "Utilities/HelperFunctions";
import HomeIcon from '@material-ui/icons/Home';
import AccountBoxIcon from '@material-ui/icons/AccountBox';
import { ROUTES } from "Utilities/Constants";
import { defaultStudentUniqueId } from "Utilities/Queries";

const navItemsInitial = [
  {label: "Home", icon: <HomeIcon color="secondary"/>, route: ROUTES.STUDENT_LIST, active: true},
  // {label: "Profile", icon: <AccountBoxIcon color="secondary"/>, route: formatRoute(ROUTES.STUDENT_PAGE, defaultStudentUsi)},
]

export default function(props) {
  const [isSidebarOpen, setIsSidebarOpen] = useState(false);
  const [navItems, setNavItems] = useState(navItemsInitial);

  const toggleSidebar = () => {
    setIsSidebarOpen(isOpen => !isOpen);
  }

  const handleMainClick = (e) => {
    if(isSidebarOpen) {
      e.stopPropagation();
      toggleSidebar();
    }
  }

  return (
    <div className="main">
      <header className="main__header"><Header toggleSidebar={toggleSidebar} isSidebarOpen={isSidebarOpen}/></header>
      <main className={"main__content" + toggledClass("blurred", isSidebarOpen)} onClick={handleMainClick}>
        <div className="student-table full-size">{props.children}</div>
      </main>
      <aside className={"main__left" + toggledClass("showing", isSidebarOpen)}>
        <NavigationBar items={navItems} setItems={setNavItems} toggle={toggleSidebar}/>
      </aside>
      <footer className="main__footer"><Footer /></footer>
    </div>
  );
};
