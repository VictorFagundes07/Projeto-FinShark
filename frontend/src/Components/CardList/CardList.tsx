import React from 'react'
import Card from '../Card/Card'

interface Props  {};

const CardList = (props: Props) => {
  return <div>
    <Card companyName="Ford" ticker="SLA" price={200} />
    <Card companyName="tesla" ticker="SLA" price={200} />
    <Card companyName="ferrari" ticker="SLA" price={200} />
    
  </div>;
  
};

export default CardList