import React, { useState } from 'react';

const PriestAvailabilityForm = () => {
  const [formData, setFormData] = useState({
    userID: '',
    startDate: '',
    endDate: '',
    days: '',
    startTime: '',
    endTime: '',
    isAvailable: false,
  });

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData({
      ...formData,
      [name]: type === 'checkbox' ? checked : value,
    });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log(formData);
    // You can add code here to send formData to your backend API
  };

  return (
    <form onSubmit={handleSubmit}>
      <div>
        <label>User ID:</label>
        <input type="text" name="userID" value={formData.userID} onChange={handleChange} />
      </div>
      <div>
        <label>Start Date:</label>
        <input type="date" name="startDate" value={formData.startDate} onChange={handleChange} />
      </div>
      <div>
        <label>End Date:</label>
        <input type="date" name="endDate" value={formData.endDate} onChange={handleChange} />
      </div>
      <div>
        <label>Days:</label>
        <input type="text" name="days" value={formData.days} onChange={handleChange} />
      </div>
      <div>
        <label>Start Time:</label>
        <input type="time" name="startTime" value={formData.startTime} onChange={handleChange} />
      </div>
      <div>
        <label>End Time:</label>
        <input type="time" name="endTime" value={formData.endTime} onChange={handleChange} />
      </div>
      <div>
        <label>Is Available:</label>
        <input type="checkbox" name="isAvailable" checked={formData.isAvailable} onChange={handleChange} />
      </div>
      <button type="submit">Submit</button>
    </form>
  );
};

export default PriestAvailabilityForm;