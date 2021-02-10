import { LogoutOutlined, UserOutlined } from "@ant-design/icons";
import { Button, Dropdown, Menu } from "antd";
import { authProvider } from "authProvider";
import { useDashboardState } from "Context/DashboardContext";
import React from "react";
import HamburgerMenu from "react-hamburger-menu";
import { LogoutHooks } from "Utilities/CustomHooks";
import "./Header.scss";

export default function Header(props) {
  const { toggleSidebar, isSidebarOpen, ...others } = props;
  const { username, authType } = useDashboardState();

  const menu = (
    <Menu>
      <Menu.Item key="1">{username}</Menu.Item>
      {authType === "Google" ? (
        <LogoutHooks />
      ) : (
        <Menu.Item key="2" onClick={() => authProvider.logout()}>
          <LogoutOutlined /> Log Out
        </Menu.Item>
      )}
    </Menu>
  );

  return (
    <div className="header__content flex-vertical-center full-size">
      <HamburgerMenu
        className="header__hamburger"
        menuClicked={toggleSidebar}
        isOpen={isSidebarOpen}
        color="white"
        animationDuration={0.2}
        strokeWidth={3}
        width={25}
        height={18}
      />
      <h3>
        <a href="/" className="color-light">
          Ed-Fi Dashboard
        </a>
      </h3>

      <Dropdown
        className="right__button"
        overlay={menu}
        placement="bottomLeft"
        trigger={["click"]}
      >
        <Button>
          <UserOutlined />
        </Button>
      </Dropdown>
    </div>
  );
}
