import PropTypes from 'prop-types';

const Render = (props) => {
  const {children, condition} = props;
  
  if(condition) {
    return children;
  } else {
    return null;
  }
};

Render.propTypes = {
  condition: PropTypes.bool,
  children: PropTypes.node.isRequired
}

Render.defaultProps = {
  condition: false
}

export default Render;