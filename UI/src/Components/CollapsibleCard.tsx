import "./CollapsibleCard.scss";
import React, { useState } from 'react';
import {Collapse} from 'react-collapse';
import { Card } from 'antd';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';

type CollapsibleCardProps = {
  title: string,
  defaultClosed?: boolean,
  children: any,
  className?: string,
  [x: string]: any
}

function CollapsibleCard(props: CollapsibleCardProps) {
  const {title, defaultClosed, children, className, ...others} = props;
  const [isOpen, setIsOpen] = useState(!defaultClosed);

  return (
    <Card title={title} 
      extra={
        <div className={"collapse-arrow " + (isOpen ? "open" : "")} >
          <ExpandMoreIcon onClick={() => setIsOpen(prev => !prev)} />
        </div>
      }
      className={(className || "") + " collapsible-card " + (isOpen ? " open" : "")}
      {...others}
    >
      <Collapse isOpened={isOpen}>
        {children}
      </Collapse>
    </Card>
  )
}

export default CollapsibleCard;