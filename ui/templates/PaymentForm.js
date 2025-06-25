'use client';

import { useState } from 'react';
import {
    useStripe,
    useElements,
    PaymentElement,
    AddressElement,
} from '@stripe/react-stripe-js';

export default function PaymentForm({ onSuccess, orderId }) {
    const stripe = useStripe();
    const elements = useElements();
    const [isLoading, setIsLoading] = useState(false);
    const [message, setMessage] = useState('');
    const [isSuccess, setIsSuccess] = useState(false);

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!stripe || !elements) {
            return;
        }

        setIsLoading(true);
        setMessage('');

        const { error } = await stripe.confirmPayment({
            elements,
            confirmParams: {
                return_url: `${window.location.origin}/payment-success`,
            },
            redirect: 'if_required',
        });

        if (error) {
            if (error.type === 'card_error' || error.type === 'validation_error') {
                setMessage(error.message || 'An error occurred');
            } else {
                setMessage('An unexpected error occurred.');
            }
        } else {
            setMessage('Payment successful! 🎉');
            setIsSuccess(true);
            setTimeout(() => {
                onSuccess();
            }, 2000);
        }

        setIsLoading(false);
    };

    const paymentElementOptions = {
        layout: 'tabs',
    };

    if (isSuccess) {
        return (
            <div className="success-container">
                <div className="success-message">
                    <h2>✅ Payment Successful!</h2>
                    <p>Thank you for your purchase!</p>
                    <p><strong>Order ID:</strong> {orderId}</p>
                    <p>You will receive a confirmation email shortly.</p>
                </div>
            </div>
        );
    }

    return (
        <form onSubmit={handleSubmit} className="payment-form">
            <div className="payment-section">
                <h3>Billing Information</h3>
                <AddressElement options={{ mode: 'billing' }} />
            </div>

            <div className="payment-section">
                <h3>Payment Information</h3>
                <PaymentElement options={paymentElementOptions} />
            </div>

            <button
                disabled={isLoading || !stripe || !elements}
                className="pay-button"
            >
                {isLoading ? (
                    <div className="spinner">
                        <div className="spinner-border"></div>
                        Processing...
                    </div>
                ) : (
                    'Pay Now'
                )}
            </button>

            {message && (
                <div className={`message ${isSuccess ? 'success' : 'error'}`}>
                    {message}
                </div>
            )}
        </form>
    );
}