import WelcomePng from '../../assets/img/welcome.png';

const Welcome = () => {
  return (
    <div className='container'>
      <img className='welcome' src={WelcomePng} alt='welcome' />
    </div>
  );
};

export default Welcome;