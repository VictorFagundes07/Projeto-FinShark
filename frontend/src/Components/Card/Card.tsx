import React from 'react'
import "./Card.css";

interface Props {
  companyName: string;
  ticker: string;
  price: number;
}

const Card = ({companyName, ticker, price}: Props) => {
  return <div className = "card">
    
    <div className='details'>
      <h2>{companyName} ({ticker})</h2>
      <p>{price}</p>
    </div>
    <p className='info'>
      blalala dakdakdakdadawkdladasldmldmakmsdmakdmad,sdmawk
    </p>
  </div>;
  
};

export default Card