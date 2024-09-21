import React, { useState, useEffect } from 'react';

const PriestAvailabilityForm = () => {
  const [availabilities, setAvailabilities] = useState([]);

  const fetchAvailabilities = async () => {
    try {
      const response = await fetch('http://localhost:5209/priestavailabilities');
      if (response.ok) {
        const data = await response.json();
        console.log('Fetched Data:', data); // Log fetched data
        setAvailabilities(data);
      } else {
        console.error('Error:', response.statusText);
      }
    } catch (error) {
      console.error('Error:', error);
    }
  };

  useEffect(() => {
    fetchAvailabilities();
  }, []);

  return (
    <div>
      <h2>Priest Availabilities</h2>
      <ul>
        {availabilities.map((availability) => (
          <li key={availability.id}>
            User ID: {availability.userID} <br />
            Start Date: {new Date(availability.startDate).toLocaleDateString()} <br />
            End Date: {new Date(availability.endDate).toLocaleDateString()} <br />
            Days: {availability.days.join(', ')} <br />
            Start Time: {availability.startTime} <br />
            End Time: {availability.endTime} <br />
            Availability: {availability.isAvailable ? 'Available' : 'Not Available'}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default PriestAvailabilityForm;