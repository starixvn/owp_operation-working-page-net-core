import { Component } from 'react';

class Loading extends Component {
  render() {
    return (
      <div className='loading-process'>
        <svg
          className='icon-loading'
          viewBox='0 0 32 32'
          width='64'
          height='64'
          fill='#439ff0'>
          <circle transform='translate(8 0)' cx='0' cy='16' r='1.29212'>
            <animate
              attributeName='r'
              values='0; 4; 0; 0'
              dur='1.2s'
              repeatCount='indefinite'
              begin='0'
              keyTimes='0;0.2;0.7;1'
              keySplines='0.2 0.2 0.4 0.8;0.2 0.6 0.4 0.8;0.2 0.6 0.4 0.8'
              calcMode='spline'
            />
          </circle>
          <circle transform='translate(16 0)' cx='0' cy='16' r='0'>
            <animate
              attributeName='r'
              values='0; 4; 0; 0'
              dur='1.2s'
              repeatCount='indefinite'
              begin='0.3'
              keyTimes='0;0.2;0.7;1'
              keySplines='0.2 0.2 0.4 0.8;0.2 0.6 0.4 0.8;0.2 0.6 0.4 0.8'
              calcMode='spline'
            />
          </circle>
          <circle transform='translate(24 0)' cx='0' cy='16' r='0.481062'>
            <animate
              attributeName='r'
              values='0; 4; 0; 0'
              dur='1.2s'
              repeatCount='indefinite'
              begin='0.6'
              keyTimes='0;0.2;0.7;1'
              keySplines='0.2 0.2 0.4 0.8;0.2 0.6 0.4 0.8;0.2 0.6 0.4 0.8'
              calcMode='spline'
            />
          </circle>
        </svg>
      </div>
    );
  }
}

export default Loading;
